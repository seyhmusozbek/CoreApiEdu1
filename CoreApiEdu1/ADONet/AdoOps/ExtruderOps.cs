using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CoreApiEdu1.ADONet.AdoEnts;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;

namespace CoreApiEdu1.ADONet.AdoOps
{
    public class ExtruderOps
    {
        private readonly IConfiguration _configuration;
        private SqlConnection _connection;


        public ExtruderOps(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new SqlConnection();
        }

        public async Task<List<ExtWorkOrder>> GetExtOrders(string machine)
        {
            List<ExtWorkOrder> extWorkOrders = new List<ExtWorkOrder>();
            try
            {
                _connection.ConnectionString = _configuration.GetConnectionString("sqlConnection");
                await OpenSql();
                _connection.ChangeDatabase("GURMENPVC2021");
                SqlCommand cmd = new SqlCommand(@"SELECT I.ID,ROW_NUMBER()OVER(ORDER BY ID)RN,[MAKINE],I.[STOK_KODU],DBO.TRK(STOK_ADI)STOKADI,
I.[MIKTAR],[ACIK1],[ACIK2],[ACIK3],[ACIK4],[ACIK5],ISNULL((SELECT SUM(P.quantity) FROM BarcodeApp..Productions P WHERE P.orderId=I.ID and P.isOkay=1),0) URETILEN 
                        FROM [GURMENPVC2021].[dbo].[FLOW_ISMRBASPR] I
                        INNER JOIN TBLSTSABIT S ON S.STOK_KODU = I.STOK_KODU
                        WHERE AKTIF = 1 AND MAKINE = @MAKINE", _connection);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@MAKINE", machine);
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    ExtWorkOrder workOrder = new ExtWorkOrder
                    {
                        Machine = reader["MAKINE"].ToString(),
                        ProductCode = reader["STOK_KODU"].ToString(),
                        ProductName = reader["STOKADI"].ToString(),
                        Exp1 = reader["ACIK1"].ToString(),
                        Exp2 = reader["ACIK2"].ToString(),
                        Exp3 = reader["ACIK3"].ToString(),
                        Exp4 = reader["ACIK4"].ToString(),
                        Exp5 = reader["ACIK5"].ToString(),
                        Planned = Convert.ToDouble(reader["MIKTAR"]),
                        Produced = Convert.ToDouble(reader["URETILEN"]),
                        Priority = Convert.ToInt32(reader["RN"]),
                        Id = Convert.ToInt32(reader["Id"]),

                    };
                    extWorkOrders.Add(workOrder);
                }
                await reader.CloseAsync();
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                await CloseSql();
            }
            return extWorkOrders;
        }

        public async Task OpenSql()
        {
            if (_connection.State == ConnectionState.Closed)
            {
                await _connection.OpenAsync();
            }
        }

        public async Task CloseSql()
        {
            if (_connection.State == ConnectionState.Open)
            {
               await _connection.CloseAsync();
            }
        }

    }
}
