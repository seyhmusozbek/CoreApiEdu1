using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CoreApiEdu1.ADONet.AdoEnts;
using CoreApiEdu1.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace CoreApiEdu1.ADONet.AdoOps
{
    public class ExtruderOps
    {
        private readonly IConfiguration _configuration;
        private SqlConnection _connection;
        private readonly Appsettings _appsettings;


        public ExtruderOps(IConfiguration configuration, IOptions<Appsettings> appsettings)
        {
            _configuration = configuration;
            _connection = new SqlConnection();
            _appsettings = appsettings.Value;
        }

        public async Task<bool> AddMachineCaliberMapping(MachineCaliberMapping request)
        {
            var connectionString = _configuration.GetConnectionString("sqlConnection");
            await using var connection = new SqlConnection(connectionString);
            var parameters = new { request.CaliberId, request.MachineId, request.Speed, request.Weight };
            var query = $"INSERT INTO dbo.CaliberMachineMappings(CaliberId, MachineId, Speed, Weight) select @CaliberId, @MachineId, @Speed, @Weight";
            var count = await connection.ExecuteAsync(query, parameters);
            return count == 1;
        }
        public async Task<List<MachineCaliberMapping>> GetMachineCaliberMappings()
        {
            var connectionString = _configuration.GetConnectionString("sqlConnection");
            await using var connection = new SqlConnection(connectionString);
            connection.ChangeDatabase(_appsettings.DatabaseName);
            return (await connection.QueryAsync<MachineCaliberMapping>("SELECT * FROM CaliberMachineMappings"))
                .AsList();
        }

        public async Task<MachineCaliberMapping> GetMachineCaliberMapping(int machineId, int caliberId)
        {
            var connectionString = _configuration.GetConnectionString("sqlConnection");
            await using var connection = new SqlConnection(connectionString);
            var parameters = new { machineId, caliberId };
            return (await connection.QueryAsync<MachineCaliberMapping>("SELECT * FROM CaliberMachineMappings where CaliberId=@caliberId and MachineId=@machineId", parameters))
                .FirstOrDefault();
        }

        public async Task<BarcodeInfo> GetBarcodeInfo(int id)
        {
            BarcodeInfo barcodeInfo = new BarcodeInfo();
            try
            {
                _connection.ConnectionString = _configuration.GetConnectionString("sqlConnection");
                await OpenSql();
                _connection.ChangeDatabase(_appsettings.DatabaseName);
                SqlCommand cmd = new SqlCommand(@"select ID,ISNULL(convert(varchar(222),M.YAPKOD),STOK_KODU)STOK_KODU,dbo.TRK(STOK_ADI)+ISNULL('-'+YAPACIK,'')STOK_ADI,DEPO_KODU,ISNULL((SELECT SUM(TOPBAKIYE) FROM YETKIN..EBASTOKBAKIYELERI A WHERE A.STOK_KODU=S.STOK_KODU AND GURMEN_DEPO_KODU=S.DEPO_KODU),0) BAKIYE,
                ISNULL(OLCU_BR2,'')BR2,ISNULL(OLCU_BR1,'')BR1,ISNULL((SELECT SUM(TOPBAKIYE/PAYDA_1) FROM YETKIN..EBASTOKBAKIYELERI A WHERE A.STOK_KODU=S.STOK_KODU AND GURMEN_DEPO_KODU=S.DEPO_KODU),0) BAKIYE2,
                case WHEN GRUP_KODU IN('02','03')THEN 'PROFIL' ELSE 'DIGER'END GRUP,B.PrDate
                FROM EFLOW_NETSIS..Barcodes B 
                LEFT JOIN TBLESNYAPMAS M ON M.YAPKOD=PrCode
                INNER JOIN TBLSTSABIT S ON S.STOK_KODU=B.PrCode OR S.STOK_KODU=YPLNDRSTOKKOD
                WHERE Id=@ID", _connection);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    barcodeInfo.barcodeId = Convert.ToInt32(reader["ID"]);
                    barcodeInfo.stokKodu = reader["STOK_KODU"].ToString();
                    barcodeInfo.stokAdi = reader["STOK_ADI"].ToString();
                    barcodeInfo.depo = Convert.ToInt32(reader["DEPO_KODU"]);
                    barcodeInfo.bakiye1 = Convert.ToDouble(reader["BAKIYE"]);
                    barcodeInfo.olcuBr1 = reader["BR1"].ToString();
                    barcodeInfo.bakiye2 = Convert.ToDouble(reader["BAKIYE2"]);
                    barcodeInfo.olcuBr2 = reader["BR2"].ToString();
                    barcodeInfo.group = reader["GRUP"].ToString();
                    barcodeInfo.basimTarihi = Convert.ToDateTime(reader["PrDate"]);
                }
                await reader.CloseAsync();
                return barcodeInfo;
            }
            catch (Exception ex)
            {
                barcodeInfo.barcodeId = 0;
                return barcodeInfo;
            }
            finally
            {
                await CloseSql();
            }
        }

        public async Task<List<BarcodeInfo>> GetStockInfos()
        {
            List<BarcodeInfo> barcodeInfos = new List<BarcodeInfo>();
            try
            {
                _connection.ConnectionString = _configuration.GetConnectionString("sqlConnection");
                await OpenSql();
                _connection.ChangeDatabase(_appsettings.DatabaseName);
                SqlCommand cmd = new SqlCommand(@"select STOK_KODU,STOK_ADI,SUM(TOPBAKIYE) BAKIYE,
                    ISNULL(OLCU_BR2,'')BR2,ISNULL(OLCU_BR1,'')BR1,ISNULL( SUM(TOPBAKIYE/PAYDA_1),0) BAKIYE2,PAY2,
                    PAYDA_1,
                    ISNULL((SELECT SUM(R.quantity1) FROM BARCODEAPP..STOCKRESERVES R  WHERE R.USEDCODE=STOK_KODU 
                    AND CODE+ORDERNUM IN(SELECT STOK_KODU+FISNO  FROM GRM_PLSIPARIS G)),0)KULLANILAN1,
					RENKLI,BOY,KATALOGKODU,KORUYUCUFOLYO,LAMINERENGI,LAMINEYUZEY,CONTARENGI,CONTATIPI,DUVARCONTA
                    FROM GRM_PLBAKIYE S 
                    GROUP BY STOK_KODU,STOK_ADI,OLCU_BR2,OLCU_BR1,PAY2,PAYDA_1,RENKLI,BOY,KATALOGKODU,KORUYUCUFOLYO,LAMINERENGI,LAMINEYUZEY,CONTARENGI,CONTATIPI,DUVARCONTA
                    HAVING SUM(TOPBAKIYE)>0", _connection);
                cmd.Parameters.Clear();
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    BarcodeInfo barcodeInfo = new BarcodeInfo();
                    barcodeInfo.stokKodu = reader["STOK_KODU"].ToString();
                    barcodeInfo.stokAdi = reader["STOK_ADI"].ToString();
                    barcodeInfo.bakiye1 = Convert.ToDouble(reader["BAKIYE"]);
                    barcodeInfo.olcuBr1 = reader["BR1"].ToString();
                    barcodeInfo.bakiye2 = Convert.ToDouble(reader["BAKIYE2"]);
                    barcodeInfo.pay2 = Convert.ToDouble(reader["PAY2"]);
                    barcodeInfo.payda1 = Convert.ToDouble(reader["PAYDA_1"]);
                    barcodeInfo.kullanilan1 = Convert.ToDouble(reader["KULLANILAN1"]);
                    barcodeInfo.olcuBr2 = reader["BR2"].ToString();
                    barcodeInfo.basimTarihi = DateTime.Now;
                    barcodeInfo.isColored = Convert.ToBoolean(Convert.ToInt32(reader["RENKLI"]));
                    barcodeInfo.length = reader["BOY"].ToString();
                    barcodeInfo.catalogCode = reader["KATALOGKODU"].ToString();
                    barcodeInfo.protectiveFoil = reader["KORUYUCUFOLYO"].ToString();
                    barcodeInfo.extColorCode = reader["LAMINERENGI"].ToString();
                    barcodeInfo.extColorFace = reader["LAMINEYUZEY"].ToString();
                    barcodeInfo.gasketColor = reader["CONTARENGI"].ToString();
                    barcodeInfo.gasketType = reader["CONTATIPI"].ToString();
                    barcodeInfo.wallGasket = reader["DUVARCONTA"].ToString();
                    barcodeInfos.Add(barcodeInfo);
                }
                await reader.CloseAsync();
                return barcodeInfos;
            }
            catch (Exception ex)
            {
                return barcodeInfos;
            }
            finally
            {
                await CloseSql();
            }
        }



        public async Task<List<BarcodeInfo>> GetOrderStockInfos(string orderNum)
        {
            List<BarcodeInfo> barcodeInfos = new List<BarcodeInfo>();
            try
            {
                _connection.ConnectionString = _configuration.GetConnectionString("sqlConnection");
                await OpenSql();
                _connection.ChangeDatabase(_appsettings.DatabaseName);
                SqlCommand cmd = new SqlCommand(@"select STOK_KODU,STOK_ADI,SUM(TOPBAKIYE)-ISNULL((SELECT SUM(R.quantity1) FROM BARCODEAPP..STOCKRESERVES R  WHERE R.USEDCODE=STOK_KODU
AND orderNum<>@ORDERNUM AND CODE+ORDERNUM IN(SELECT STOK_KODU+FISNO FROM GRM_PLSIPARIS G)),0) BAKIYE,
ISNULL(OLCU_BR2,'')BR2,ISNULL(OLCU_BR1,'')BR1,ISNULL(SUM(TOPBAKIYE)-ISNULL((SELECT SUM(R.quantity1) FROM BARCODEAPP..STOCKRESERVES R  WHERE R.USEDCODE=STOK_KODU
AND orderNum<>@ORDERNUM AND CODE+ORDERNUM IN(SELECT STOK_KODU+FISNO FROM GRM_PLSIPARIS G)),0),0)/PAYDA_1 BAKIYE2,PAY2,PAYDA_1,
ISNULL((SELECT SUM(R.quantity1) FROM BARCODEAPP..STOCKRESERVES R  WHERE R.USEDCODE=STOK_KODU
AND CODE+ORDERNUM IN(SELECT STOK_KODU+FISNO FROM GRM_PLSIPARIS WHERE FISNO=@ORDERNUM)),0)KULLANILAN1,
RENKLI,BOY,KATALOGKODU,KORUYUCUFOLYO,LAMINERENGI,LAMINEYUZEY,CONTARENGI,CONTATIPI,DUVARCONTA
FROM GRM_PLBAKIYE S 
WHERE STOK_KODU IN(SELECT STOK_KODU FROM GRM_PLSIPARIS WHERE FISNO=@ORDERNUM)
GROUP BY STOK_KODU,STOK_ADI,OLCU_BR2,OLCU_BR1,PAY2,PAYDA_1,RENKLI,BOY,KATALOGKODU,KORUYUCUFOLYO,LAMINERENGI,LAMINEYUZEY,CONTARENGI,CONTATIPI,DUVARCONTA
HAVING SUM(TOPBAKIYE)>0
", _connection);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@ORDERNUM", orderNum);
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    BarcodeInfo barcodeInfo = new BarcodeInfo();
                    barcodeInfo.stokKodu = reader["STOK_KODU"].ToString();
                    barcodeInfo.stokAdi = reader["STOK_ADI"].ToString();
                    barcodeInfo.bakiye1 = Convert.ToDouble(reader["BAKIYE"]);
                    barcodeInfo.olcuBr1 = reader["BR1"].ToString();
                    barcodeInfo.bakiye2 = Convert.ToDouble(reader["BAKIYE2"]);
                    barcodeInfo.pay2 = Convert.ToDouble(reader["PAY2"]);
                    barcodeInfo.olcuBr2 = reader["BR2"].ToString();
                    barcodeInfo.kullanilan1 = Convert.ToDouble(reader["KULLANILAN1"]);
                    barcodeInfo.payda1 = Convert.ToDouble(reader["PAYDA_1"]);
                    barcodeInfo.isColored = Convert.ToBoolean(Convert.ToInt32(reader["RENKLI"]));
                    barcodeInfo.length = reader["BOY"].ToString();
                    barcodeInfo.catalogCode = reader["KATALOGKODU"].ToString();
                    barcodeInfo.protectiveFoil = reader["KORUYUCUFOLYO"].ToString();
                    barcodeInfo.extColorCode = reader["LAMINERENGI"].ToString();
                    barcodeInfo.extColorFace = reader["LAMINEYUZEY"].ToString();
                    barcodeInfo.gasketColor = reader["CONTARENGI"].ToString();
                    barcodeInfo.gasketType = reader["CONTATIPI"].ToString();
                    barcodeInfo.wallGasket = reader["DUVARCONTA"].ToString();
                    barcodeInfos.Add(barcodeInfo);
                }
                await reader.CloseAsync();
                return barcodeInfos;
            }
            catch (Exception ex)
            {
                return barcodeInfos;
            }
            finally
            {
                await CloseSql();
            }
        }
        public async Task<List<AddWHTransferDTO>> GetTransactions(string code)
        {
            List<AddWHTransferDTO> transactions = new List<AddWHTransferDTO>();
            try
            {
                _connection.ConnectionString = _configuration.GetConnectionString("sqlConnection");
                await OpenSql();
                _connection.ChangeDatabase(_appsettings.DatabaseName);
                SqlCommand cmd = new SqlCommand(@"select CASE WHEN STHAR_HTUR IN('J','H') THEN DBO.TRK(CARI_ISIM) ELSE STHAR_ACIKLAMA END
                ACIKLAMA,STHAR_GCMIK*CASE WHEN STHAR_GCKOD='G' THEN 1 ELSE -1 END/PAYDA_1 PKMIK,
                STHAR_TARIH
                FROM TBLSTHAR H
                INNER JOIN TBLSTSABIT S ON S.STOK_KODU=H.STOK_KODU
                LEFT JOIN TBLCASABIT C ON C.CARI_KOD=STHAR_ACIKLAMA
                WHERE (H.STOK_KODU LIKE @KOD OR h.YAPKOD LIKE @KOD)
                AND STHAR_TARIH>GETDATE()-7
                AND H.DEPO_KODU IN(130,132,200,400,410,210) and STHAR_ACIKLAMA<>'410-410'
                AND H.STHAR_GCKOD='G'
                AND STHAR_GCMIK>0
                ORDER BY STHAR_TARIH ASC", _connection);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@KOD", code);
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    AddWHTransferDTO transaction = new AddWHTransferDTO();
                    transaction.aciklama = reader["ACIKLAMA"].ToString();
                    transaction.date = Convert.ToDateTime(reader["STHAR_TARIH"]);
                    transaction.miktar = Convert.ToDouble(reader["PKMIK"]);
                    transactions.Add(transaction);
                }
                await reader.CloseAsync();
                return transactions;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                await CloseSql();
            }
        }

        public async Task<List<CustomerOrder>> GetCustOrders()
        {
            var connectionString = _configuration.GetConnectionString("sqlConnection");
            await using var connection = new SqlConnection(connectionString);
            return (await connection.QueryAsync<CustomerOrder>(@"select FISNO orderNum,UNVAN customerName,FISNO2 orderNum2,BELGENOTU exp1,SUM(KALAN_MIKTAR*CEVRIM3)KALANKG,SUM(KALAN_MIKTAR*CEVRIM2)KALANPK,
  CONVERT(bit,CASE WHEN(SELECT COUNT(*)  FROM BarcodeApp..ChosenOrders  where ORDERNUM=FISNO )>0 THEN 1
  ELSE 0 END) chosen,ISNULL((select TOP 1 CASE WHEN C.[priority]=0 THEN 9999 ELSE C.[priority] END  FROM BarcodeApp..ChosenOrders C WHERE C.orderNum=FISNO ),9999) [priority],
  (select top 1 [merge] from BarcodeApp..ChosenOrders c where c.orderNum = FISNO)[merge]
  from GRM_PLSIPARIS WHERE KALAN_MIKTAR>0 
  GROUP BY FISNO,UNVAN,FISNO2,BELGENOTU
  HAVING SUM(KALAN_MIKTAR*CEVRIM2)>=1"))
                .AsList();
        }


        public async Task<List<AddStockReserveDTO>> GetStockReserves()
        {
            List<AddStockReserveDTO> stockReserves = new List<AddStockReserveDTO>();
            try
            {
                _connection.ConnectionString = _configuration.GetConnectionString("sqlConnection");
                await OpenSql();
                _connection.ChangeDatabase(_appsettings.DatabaseName);
                SqlCommand cmd = new SqlCommand(@"  SELECT [code],[orderNum],[quantity1],[usedCode] FROM [BarcodeApp].[dbo].[StockReserves]
  where code+orderNum IN(SELECT STOK_KODU+FISNO FROM GRM_PLSIPARIS )", _connection);
                cmd.Parameters.Clear();
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    AddStockReserveDTO stockRes = new AddStockReserveDTO();
                    stockRes.orderNum = reader["orderNum"].ToString();
                    stockRes.code = reader["code"].ToString();
                    stockRes.quantity1 = Convert.ToDouble(reader["quantity1"]);
                    stockRes.usedCode = reader["usedCode"].ToString();
                    stockReserves.Add(stockRes);
                }
                await reader.CloseAsync();
                return stockReserves;
            }
            catch (Exception ex)
            {
                return stockReserves;
            }
            finally
            {
                await CloseSql();
            }
        }

        public async Task<List<Caliber>> GetCalibers()
        {
            var connectionString = _configuration.GetConnectionString("sqlConnection");
            await using var connection = new SqlConnection(connectionString);
            var calibers = (await connection.QueryAsync<Caliber>(@"select Id ,Code code,Description name,Exp1 catalogCode,ISNULL(Speed,1) speed,ISNULL(Weight,1000) weight, isnull([Count],0)[Count],case when len(Code)>4 then 'ANA' ELSE 'YARDIMCI' END prType
FROM Calibers where IsDeleted=0"))
                .AsList();
            foreach (var cal in calibers)
            {
                var parameters = new { cal.code };
                var isAbleTo = (await connection.QueryAsync<MachineCaliberMappingDetails>(@"select  m.machineName,max(cm.speed)speed from CaliberMachineMappings cm inner join Calibers c on c.Id=CaliberId inner join Machines m on m.id=MachineId where c.Code=@code group by m.machineName", parameters))
               .AsList();
                cal.isAbleTo = isAbleTo;
            }

            return calibers;
        }

      
        public async Task<List<CustomerOrderDetail>> GetCOrderDetails(string orderNum)
        {
            var connectionString = _configuration.GetConnectionString("sqlConnection");
            await using var connection = new SqlConnection(connectionString);
            var parameters = new { orderNum };
            return (await connection.QueryAsync<CustomerOrderDetail>(@"SELECT ISNULL((SELECT TOP 1 STEXT FROM PROLINE..IASMATX WHERE TEXTTYPE='B' AND LANGU='T' AND VALIDUNTIL >=GETDATE() AND MATERIAL=subCode),'MALZEME ADI BULUNAMADI')subName,* FROM(
                    SELECT STOK_KOD code,STOK_ADI name,GURMENFISNO orderNum,BR1 unit1,KALAN_MIKTAR quantity1,BR2 unit2,SIPKALAN_MIKTAR quantity2,ACIKLAMA1 exp1,
                    BR3 unit3,KALANKG quantity3,
                    ISNULL((SELECT SUM(R.quantity1) FROM BARCODEAPP..STOCKRESERVES R  WHERE CODE+ORDERNUM=STOK_KOD+GURMENFISNO),0)readyQuantity,
                    ISNULL((SELECT STEXT FROM [PROLINE].[dbo].[IASBAS070X] WHERE CLIENT='00' AND COMPANY='01' AND ATTRIBUTE='PRFLHMRRENK' AND LANGU='T' AND VALUE=SUBSTRING(STOK_KOD,8,2)),'Beyaz')dColor,
                    ISNULL((SELECT TOP 1 COMPONENT FROM PROLINE..IASBOMITEM WHERE CLIENT='00' AND COMPANY='01' AND PLANT='01' AND COMPONENT LIKE 'PR%' AND MATERIAL=STOK_KOD),'-')subCode,
                    RENKLI isColored,BOY length,KATALOGKODU catalogCode,KORUYUCUFOLYO protectiveFoil,LAMINERENGI extColorCode,LAMINEYUZEY extColorFace,CONTARENGI gasketColor,CONTATIPI gasketType,DUVARCONTA wallGasket,CALIBER caliberCode
                    FROM (                   
                    SELECT STOK_KODU URETICI_KODU, STOK_KODU  STOK_KOD,        
                    STOK_ADI  STOK_ADI,        
                    FISNO GURMENFISNO,BR2,X.KALAN_MIKTAR,BR1,BR3,X.KALAN_MIKTAR*CEVRIM2 SIPKALAN_MIKTAR,0 KULL_BAKIYE,0 ACIKISEMRI,X.KALAN_MIKTAR*CEVRIM3 KALANKG,                  
                    0 REZSTOK,0 REZISMR,0 PLANMIKTAR,RECETE,'Uygun'UYGUN,ISNULL(x.ACIKLAMA,'-')+' '+UNVAN ACIKLAMA1,SIRANO ID,RENKLI,BOY,KATALOGKODU,KORUYUCUFOLYO,LAMINERENGI,LAMINEYUZEY,CONTARENGI,CONTATIPI,DUVARCONTA,CALIBER
                    FROM(         
                    SELECT S.*,CASE WHEN (SELECT COUNT(*) FROM PROLINE..IASBOMITEM WHERE CLIENT='00' AND COMPANY='01' AND PLANT='01' AND MATERIAL=S.STOK_KODU)>0              
                    THEN 'VAR' else 'YOK' END RECETE 
                    FROM GRM_PLSIPARIS   S  
                    WHERE FISNO=@ORDERNUM
                    )X     
                    WHERE KALAN_MIKTAR>0       
                    )Y  
                    )X", parameters))
                .AsList();
        }

        public async Task<List<CustomerOrderDetail>> GetCOrderDetails(List<string> orderNum)
        {
            var connectionString = _configuration.GetConnectionString("sqlConnection");
            await using var connection = new SqlConnection(connectionString);
            var parameters = new { orderNum };
            return (await connection.QueryAsync<CustomerOrderDetail>(@"SELECT ISNULL((SELECT TOP 1 STEXT FROM PROLINE..IASMATX WHERE TEXTTYPE='B' AND LANGU='T' AND VALIDUNTIL >=GETDATE() AND MATERIAL=subCode),'MALZEME ADI BULUNAMADI')subName,* FROM(
                    SELECT STOK_KOD code,STOK_ADI name,GURMENFISNO orderNum,BR1 unit1,KALAN_MIKTAR quantity1,BR2 unit2,SIPKALAN_MIKTAR quantity2,ACIKLAMA1 exp1,
                    BR3 unit3,KALANKG quantity3,
                    ISNULL((SELECT SUM(R.quantity1) FROM BARCODEAPP..STOCKRESERVES R  WHERE CODE+ORDERNUM=STOK_KOD+GURMENFISNO),0)readyQuantity,
                    ISNULL((SELECT STEXT FROM [PROLINE].[dbo].[IASBAS070X] WHERE CLIENT='00' AND COMPANY='01' AND ATTRIBUTE='PRFLHMRRENK' AND LANGU='T' AND VALUE=SUBSTRING(STOK_KOD,8,2)),'Beyaz')dColor,
                    ISNULL((SELECT TOP 1 COMPONENT FROM PROLINE..IASBOMITEM WHERE CLIENT='00' AND COMPANY='01' AND PLANT='01' AND COMPONENT LIKE 'PR%' AND MATERIAL=STOK_KOD),'-')subCode,
                    RENKLI isColored,BOY length,KATALOGKODU catalogCode,KORUYUCUFOLYO protectiveFoil,LAMINERENGI extColorCode,LAMINEYUZEY extColorFace,CONTARENGI gasketColor,CONTATIPI gasketType,DUVARCONTA wallGasket,CALIBER caliberCode
                    FROM (                   
                    SELECT STOK_KODU URETICI_KODU, STOK_KODU  STOK_KOD,        
                    STOK_ADI  STOK_ADI,        
                    FISNO GURMENFISNO,BR2,X.KALAN_MIKTAR,BR1,BR3,X.KALAN_MIKTAR*CEVRIM2 SIPKALAN_MIKTAR,0 KULL_BAKIYE,0 ACIKISEMRI,X.KALAN_MIKTAR*CEVRIM3 KALANKG,                  
                    0 REZSTOK,0 REZISMR,0 PLANMIKTAR,RECETE,'Uygun'UYGUN,ISNULL(x.ACIKLAMA,'-')+' '+UNVAN ACIKLAMA1,SIRANO ID,RENKLI,BOY,KATALOGKODU,KORUYUCUFOLYO,LAMINERENGI,LAMINEYUZEY,CONTARENGI,CONTATIPI,DUVARCONTA,CALIBER
                    FROM(         
                    SELECT S.*,CASE WHEN (SELECT COUNT(*) FROM PROLINE..IASBOMITEM WHERE CLIENT='00' AND COMPANY='01' AND PLANT='01' AND MATERIAL=S.STOK_KODU)>0              
                    THEN 'VAR' else 'YOK' END RECETE 
                    FROM GRM_PLSIPARIS   S  
                    WHERE FISNO in @ORDERNUM
                    )X     
                    WHERE KALAN_MIKTAR>0       
                    )Y  
                    )X", parameters))
                .AsList();
        }


        public async Task<List<ExtWorkOrder>> GetExtOrders(string machine)
        {
            List<ExtWorkOrder> extWorkOrders = new List<ExtWorkOrder>();
            try
            {
                _connection.ConnectionString = _configuration.GetConnectionString("sqlConnection");
                await OpenSql();
                _connection.ChangeDatabase(_appsettings.DatabaseName);
                SqlCommand cmd = new SqlCommand(@"SELECT I.ID,ROW_NUMBER()OVER(ORDER BY ID)RN,[MAKINE],I.[STOK_KODU],DBO.TRK(STOK_ADI)STOKADI,
I.[MIKTAR],[ACIK1],[ACIK2],[ACIK3],[ACIK4],[ACIK5],ISNULL((SELECT SUM(P.quantity) FROM BarcodeApp..Productions P WHERE P.orderId=I.ID and P.isOkay=1),0) URETILEN,CHOSEN
                        FROM [dbo].[FLOW_ISMRBASPR] I
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
                        Chosen = Convert.ToBoolean(reader["CHOSEN"]),

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

        public async Task ChooseOrder(Chosen chosen)
        {
            try
            {
                _connection.ConnectionString = _configuration.GetConnectionString("sqlConnection");
                await OpenSql();
                _connection.ChangeDatabase(_appsettings.DatabaseName);

                SqlCommand cmd = new SqlCommand(@"update FLOW_ISMRBASPR SET CHOSEN=CASE WHEN @ID=ID THEN 1 ELSE 0 END WHERE MAKINE=@MAKINE AND AKTIF=1", _connection);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@MAKINE", chosen.machine);
                cmd.Parameters.AddWithValue("@ID", chosen.orderId);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                await CloseSql();
            }
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

        public async Task<bool> RemoveMachineCaliberMapping(MachineCaliberMapping machineCaliberMappingRequest)
        {
            var connectionString = _configuration.GetConnectionString("sqlConnection");
            await using var connection = new SqlConnection(connectionString);
            var query = $"delete from dbo.CaliberMachineMappings where CaliberId=@CaliberId and MachineId=@MachineId";
            var parameters = new { machineCaliberMappingRequest.CaliberId, machineCaliberMappingRequest.MachineId };
            var count = await connection.ExecuteAsync(query, parameters);
            return count == 1;
        }
    }
}
