using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using CoreApiEdu1.ADONet.AdoEnts;
using CoreApiEdu1.Entities;
using CoreApiEdu1.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CoreApiEdu1.ADONet.AdoOps
{
    public class WMSOps
    {
        private readonly IConfiguration _configuration;
        private SqlConnection _connection;



        public WMSOps(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new SqlConnection();
        }
        public async Task WMSInsertToERP2(List<WHTransfer> transfers)
        {
            SqlTransaction transaction = null;
            try
            {
                _connection.ConnectionString = _configuration.GetConnectionString("sqlConnection");
                await OpenSql();
                _connection.ChangeDatabase("GURMENPVC2021");
                transaction = _connection.BeginTransaction();//transaction begin

                SqlCommand cmd = new SqlCommand(@"SELECT ISNULL((SELECT TOP 1 'B'+@YIL+RIGHT('000000000'+CONVERT(VARCHAR(15),RIGHT(GURMENFISNO,5)+1),10) FROM (
SELECT FATIRSNO GURMENFISNO FROM FLOW_FISNORES WHERE TIPI='DT' AND FATIRSNO LIKE 'B%' AND LEN(FATIRSNO)>10
)Z WHERE SUBSTRING(GURMENFISNO,2,4)=@YIL
ORDER BY GURMENFISNO DESC),'B'+@YIL+'0000000001') as FISNO", _connection, transaction);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@YIL", DateTime.Now.Year.ToString());

                string fisno = "";
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (reader.Read())
                {
                    fisno = reader["FISNO"].ToString();
                }
                await reader.CloseAsync();

                cmd.CommandText = @"INSERT INTO FLOW_FISNORES(FATIRSNO,TIPI) SELECT @FISNO,'DT'";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@FISNO", fisno);
                await cmd.ExecuteNonQueryAsync();

                cmd.CommandText = @"INSERT INTO TBLFATUIRS(SUBE_KODU,FTIRSIP,FATIRS_NO,ACIKLAMA,CARI_KODU,TARIH,
                ODEMETARIHI,FATKALEM_ADEDI,TIPI,ISLETME_KODU,C_YEDEK6,YEDEK)
                SELECT 0,'8',@FISNO,'Barkod','000000000000000',convert(date,GETDATE()),convert(date,GETDATE()),
                @COUNT,0,1,'B','D'";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@FISNO", fisno);
                cmd.Parameters.AddWithValue("@COUNT", transfers.Count);
                await cmd.ExecuteNonQueryAsync();

                short rownumber = 0;
                foreach (var item in transfers)
                {
                    if (item.stokKodu.StartsWith("YMM") && item.cikDepo == 110)
                    {
                        item.girDepo = 120;
                    }
                    rownumber++;
                    for (int i = 0; i < 2; i++)
                    {
                        cmd.CommandText = @"INSERT INTO TBLSTHAR (STOK_KODU,STHAR_GCMIK,FISNO,STHAR_TARIH,DEPO_KODU,STHAR_GCKOD,STHAR_ACIKLAMA,
                        OLCUBR,SUBE_KODU,STHAR_HTUR,STHAR_BGTIP,STHAR_FTIRSIP,I_YEDEK8,CEVRIM,EKALAN1,SIRA,C_YEDEK6,EKALAN,YAPKOD)
                        SELECT STOK_KODU,@MIKTAR*CASE WHEN @OBR=2 THEN PAYDA_1 WHEN @OBR=1 THEN 1 ELSE RIGHT(STOK_KODU,3)/100.00 END,@FISNO,convert(date,GETDATE()),
                        CASE WHEN @DEPO=130 AND @STOKKODU LIKE 'YMM%' THEN 120 ELSE @DEPO END,@GC,@ACIK,1,0,'B','I',@FTIRSIP,@DEPO2,
                        0,@ID,@SIRA,'D',LEFT(@EK1,50),M.YAPKOD FROM TBLSTSABIT 
						LEFT JOIN TBLESNYAPMAS M ON M.YPLNDRSTOKKOD=STOK_KODU
                        WHERE STOK_KODU=@STOKKODU OR M.YAPKOD=@STOKKODU";

                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@STOKKODU", item.stokKodu);
                        cmd.Parameters.AddWithValue("@MIKTAR", item.miktar);
                        cmd.Parameters.AddWithValue("@FISNO", fisno);
                        cmd.Parameters.AddWithValue("@DEPO", i == 0 ? item.cikDepo : item.girDepo);
                        cmd.Parameters.AddWithValue("@GC", i == 0 ? "C" : "G");
                        cmd.Parameters.AddWithValue("@ACIK", item.cikDepo.ToString() + "-" + item.girDepo.ToString());
                        cmd.Parameters.AddWithValue("@FTIRSIP", i == 0 ? "8" : "9");
                        cmd.Parameters.AddWithValue("@DEPO2", i == 0 ? item.girDepo : item.cikDepo);
                        cmd.Parameters.AddWithValue("@ID", item.id);
                        cmd.Parameters.AddWithValue("@SIRA", rownumber);
                        cmd.Parameters.AddWithValue("@OBR", item.obr);
                        cmd.Parameters.AddWithValue("@EK1", item.aciklama);

                        var result = await cmd.ExecuteNonQueryAsync();
                        if (result == 0) throw new NotImplementedException();
                    }
                    cmd.CommandText = @"update BarcodeApp..WHTransfers SET isSaved=1 where id=@id";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ID", item.id);
                    cmd.ExecuteNonQuery();
                }


                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                transaction.Dispose();
                throw;
            }
            finally
            {
                transaction.Dispose();
                await CloseSql();
            }
        }

        public async Task WMSInsertToERP(List<WHTransfer> transfers)
        {
            SqlTransaction transaction = null;
            try
            {
                _connection.ConnectionString = _configuration.GetConnectionString("sqlConnection");
                await OpenSql();
                _connection.ChangeDatabase("GURMENPVC2021");
                transaction = _connection.BeginTransaction();//transaction begin

                SqlCommand cmd = new SqlCommand(@"SELECT ISNULL((SELECT TOP 1 'B'+@YIL+RIGHT('000000000'+CONVERT(VARCHAR(15),RIGHT(GURMENFISNO,5)+1),10) FROM (
                SELECT FATIRSNO GURMENFISNO FROM FLOW_FISNORES WHERE TIPI='DT' AND FATIRSNO LIKE 'B%' AND LEN(FATIRSNO)>10
                union all select FATIRS_NO FROM TBLFATUIRS WHERE FTIRSIP='8' AND FATIRS_NO LIKE 'B%'
                )Z WHERE SUBSTRING(GURMENFISNO,2,4)=@YIL
                ORDER BY GURMENFISNO DESC),'B'+@YIL+'0000000001') as FISNO", _connection, transaction);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@YIL", DateTime.Now.Year.ToString());

                string fisno = "";
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (reader.Read())
                {
                    fisno = reader["FISNO"].ToString();
                }
                await reader.CloseAsync();

                cmd.CommandText = @"INSERT INTO FLOW_FISNORES(FATIRSNO,TIPI) SELECT @FISNO,'DT'";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@FISNO", fisno);
                await cmd.ExecuteNonQueryAsync();

                cmd.CommandText = @"update TBLFATUNO set NUMARA= @FISNO WHERE SUBE_KODU=0
                AND TIP='8' AND SERI IS NULL AND ALTTIP IS NULL";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@FISNO", fisno);
                await cmd.ExecuteNonQueryAsync();

                cmd.CommandText = @"INSERT INTO TBLFATUIRS(SUBE_KODU,FTIRSIP,FATIRS_NO,ACIKLAMA,CARI_KODU,TARIH,
                ODEMETARIHI,FATKALEM_ADEDI,TIPI,ISLETME_KODU,C_YEDEK6,YEDEK)
                SELECT 0,'8',@FISNO,'Barkod','000000000000000',convert(date,GETDATE()),convert(date,GETDATE()),
                @COUNT,0,1,'B','D'";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@FISNO", fisno);
                cmd.Parameters.AddWithValue("@COUNT", transfers.Count);
                await cmd.ExecuteNonQueryAsync();

                short rownumber = 0;
                foreach (var item in transfers)
                {
                    if (item.stokKodu.StartsWith("YMM") && item.cikDepo == 110)
                    {
                        item.girDepo = 120;
                    }
                    rownumber++;
                    for (int i = 0; i < 2; i++)
                    {
                        cmd.CommandText = @"INSERT INTO TBLSTHAR (STOK_KODU,STHAR_GCMIK,FISNO,STHAR_TARIH,DEPO_KODU,STHAR_GCKOD,STHAR_ACIKLAMA,
                        OLCUBR,SUBE_KODU,STHAR_HTUR,STHAR_BGTIP,STHAR_FTIRSIP,I_YEDEK8,CEVRIM,EKALAN1,SIRA,C_YEDEK6,EKALAN,YAPKOD)
                        SELECT STOK_KODU,@MIKTAR*CASE WHEN @OBR=2 THEN PAYDA_1 WHEN @OBR=1 THEN 1 ELSE RIGHT(STOK_KODU,3)/100.00 END,@FISNO,convert(date,GETDATE()),
                        CASE WHEN @DEPO=130 AND @STOKKODU LIKE 'YMM%' THEN 120 ELSE @DEPO END,@GC,@ACIK,1,0,'B','I',@FTIRSIP,@DEPO2,
                        0,@ID,@SIRA,'D',LEFT(@EK1,50),M.YAPKOD FROM TBLSTSABIT 
						LEFT JOIN TBLESNYAPMAS M ON M.YPLNDRSTOKKOD=STOK_KODU
                        WHERE STOK_KODU=@STOKKODU OR M.YAPKOD=@STOKKODU";

                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@STOKKODU", item.stokKodu);
                        cmd.Parameters.AddWithValue("@MIKTAR", item.miktar);
                        cmd.Parameters.AddWithValue("@FISNO", fisno);
                        cmd.Parameters.AddWithValue("@DEPO", i == 0 ? item.cikDepo : item.girDepo);
                        cmd.Parameters.AddWithValue("@GC", i == 0 ? "C" : "G");
                        cmd.Parameters.AddWithValue("@ACIK", item.cikDepo.ToString() + "-" + item.girDepo.ToString());
                        cmd.Parameters.AddWithValue("@FTIRSIP", i == 0 ? "8" : "9");
                        cmd.Parameters.AddWithValue("@DEPO2", i == 0 ? item.girDepo : item.cikDepo);
                        cmd.Parameters.AddWithValue("@ID", item.id);
                        cmd.Parameters.AddWithValue("@SIRA", rownumber);
                        cmd.Parameters.AddWithValue("@OBR", item.obr);
                        cmd.Parameters.AddWithValue("@EK1", item.aciklama);

                        var result = await cmd.ExecuteNonQueryAsync();
                        if (result == 0) throw new NotImplementedException();
                    }
                    cmd.CommandText = @"update BarcodeApp..WHTransfers SET isSaved=1 where id=@id";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ID", item.id);
                    cmd.ExecuteNonQuery();
                }


                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                transaction.Dispose();
                throw;
            }
            finally
            {
                transaction.Dispose();
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

    }
}
