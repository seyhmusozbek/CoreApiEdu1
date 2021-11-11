using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using CoreApiEdu1.ADONet.AdoEnts;
using CoreApiEdu1.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

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

        public async Task<BarcodeInfo> GetBarcodeInfo(int id)
        {
            BarcodeInfo barcodeInfo = new BarcodeInfo();
            try
            {
                _connection.ConnectionString = _configuration.GetConnectionString("sqlConnection");
                await OpenSql();
                _connection.ChangeDatabase("GURMENPVC2021");
                SqlCommand cmd = new SqlCommand(@"select ID,ISNULL(convert(varchar(222),M.YAPKOD),STOK_KODU)STOK_KODU,dbo.TRK(STOK_ADI)+ISNULL('-'+YAPACIK,'')STOK_ADI,DEPO_KODU,ISNULL((SELECT SUM(TOPBAKIYE) FROM YETKIN..EBASTOKBAKIYELERI A WHERE A.STOK_KODU=S.STOK_KODU AND GURMEN_DEPO_KODU=S.DEPO_KODU),0) BAKIYE,
                ISNULL(OLCU_BR2,'')BR2,ISNULL(OLCU_BR1,'')BR1,ISNULL((SELECT SUM(TOPBAKIYE/PAYDA_1) FROM YETKIN..EBASTOKBAKIYELERI A WHERE A.STOK_KODU=S.STOK_KODU AND GURMEN_DEPO_KODU=S.DEPO_KODU),0) BAKIYE2,
                case WHEN GRUP_KODU IN('02','03')THEN 'PROFIL' ELSE 'DIGER'END GRUP,B.PrDate
                FROM EFLOW_NETSIS..Barcodes B 
                LEFT JOIN GURMENPVC2021..TBLESNYAPMAS M ON M.YAPKOD=PrCode
                INNER JOIN GURMENPVC2021..TBLSTSABIT S ON S.STOK_KODU=B.PrCode OR S.STOK_KODU=YPLNDRSTOKKOD
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
                    barcodeInfo.basimTarihi=Convert.ToDateTime(reader["PrDate"]);
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
            List<BarcodeInfo> barcodeInfos= new List<BarcodeInfo>();
            try
            {
                _connection.ConnectionString = _configuration.GetConnectionString("sqlConnection");
                await OpenSql();
                _connection.ChangeDatabase("GURMENPVC2021");
                SqlCommand cmd = new SqlCommand(@"select STOK_KODU,dbo.TRK(STOK_ADI)STOK_ADI,SUM(TOPBAKIYE) BAKIYE,
ISNULL(OLCU_BR2,'')BR2,ISNULL(OLCU_BR1,'')BR1,ISNULL( SUM(TOPBAKIYE/PAYDA_1),0) BAKIYE2,PAY2,
PAYDA_1,
ISNULL((SELECT SUM(R.QUANTITY1) FROM BARCODEAPP..STOCKRESERVES R  WHERE R.USEDCODE=STOK_KODU
AND CODE+ORDERNUM IN(SELECT STOK_KODU+FISNO FROM GRM_PLSIPARIS G WHERE G.GRUP_KODU IN('03'))),0)KULLANILAN1
FROM YETKIN..EBASTOKBAKIYELERI S 
WHERE S.GURMEN_DEPO_KODU IN(130,132,120,200) and STOK_KODU LIKE '_MM%'
GROUP BY STOK_KODU,STOK_ADI,OLCU_BR2,OLCU_BR1,PAY2,PAYDA_1
HAVING SUM(TOPBAKIYE)>0
", _connection);
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
                _connection.ChangeDatabase("GURMENPVC2021");
                SqlCommand cmd = new SqlCommand(@"select STOK_KODU,dbo.TRK(STOK_ADI)STOK_ADI,SUM(TOPBAKIYE)-ISNULL((SELECT SUM(R.QUANTITY1) FROM BARCODEAPP..STOCKRESERVES R  WHERE R.USEDCODE=STOK_KODU
AND orderNum<>@ORDERNUM AND CODE+ORDERNUM IN(SELECT STOK_KODU+FISNO FROM GRM_PLSIPARIS G WHERE G.GRUP_KODU IN('03'))),0) BAKIYE,
ISNULL(OLCU_BR2,'')BR2,ISNULL(OLCU_BR1,'')BR1,ISNULL(SUM(TOPBAKIYE)-ISNULL((SELECT SUM(R.QUANTITY1) FROM BARCODEAPP..STOCKRESERVES R  WHERE R.USEDCODE=STOK_KODU
AND orderNum<>@ORDERNUM AND CODE+ORDERNUM IN(SELECT STOK_KODU+FISNO FROM GRM_PLSIPARIS G WHERE G.GRUP_KODU IN('03'))),0),0)/PAYDA_1 BAKIYE2,PAY2,PAYDA_1,
ISNULL((SELECT SUM(R.QUANTITY1) FROM BARCODEAPP..STOCKRESERVES R  WHERE R.USEDCODE=STOK_KODU
AND CODE+ORDERNUM IN(SELECT STOK_KODU+FISNO FROM GRM_PLSIPARIS WHERE FISNO=@ORDERNUM)),0)KULLANILAN1
FROM YETKIN..EBASTOKBAKIYELERI S 
WHERE S.GURMEN_DEPO_KODU IN(130,132,120,200)
and LEFT(STOK_KODU,20)IN(SELECT LEFT(STOK_KODU,20) FROM GRM_PLSIPARIS WHERE FISNO=@ORDERNUM)
GROUP BY STOK_KODU,STOK_ADI,OLCU_BR2,OLCU_BR1,PAY2,PAYDA_1
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
                    barcodeInfo.kullanilan1= Convert.ToDouble(reader["KULLANILAN1"]);
                    barcodeInfo.payda1 = Convert.ToDouble(reader["PAYDA_1"]);
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
                _connection.ChangeDatabase("GURMENPVC2021");
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
                cmd.Parameters.AddWithValue("@KOD",code);
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
            List<CustomerOrder> custOrders = new List<CustomerOrder>();
            try
            {
                _connection.ConnectionString = _configuration.GetConnectionString("sqlConnection");
                await OpenSql();
                _connection.ChangeDatabase("GURMENPVC2021");
                SqlCommand cmd = new SqlCommand(@"select FISNO,UNVAN ACIKLAMA,FISNO2,BELGENOTU,SUM(KALAN_MIKTAR*CEVRIM3)KALANKG,SUM(KALAN_MIKTAR*CEVRIM2)KALANPK,
  CONVERT(bit,CASE WHEN(SELECT COUNT(*)  FROM BarcodeApp..ChosenOrders  where ORDERNUM=FISNO)>0 THEN 1
  ELSE 0 END) ISCHOSEN,ISNULL((select TOP 1 CASE WHEN C.[priority]=0 THEN 9999 ELSE C.[priority] END  FROM BarcodeApp..ChosenOrders C WHERE C.orderNum=FISNO),9999)PRIOR 
  from GRM_PLSIPARIS WHERE GRUP_KODU  IN('03','02') AND KALAN_MIKTAR>0 AND FISNO NOT LIKE 'D2%'
  GROUP BY FISNO,UNVAN,FISNO2,BELGENOTU
  HAVING SUM(KALAN_MIKTAR*CEVRIM2)>=1
", _connection);
                cmd.Parameters.Clear();
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    CustomerOrder custOrder = new CustomerOrder();
                    custOrder.orderNum = reader["FISNO"].ToString();
                    custOrder.customerName = reader["ACIKLAMA"].ToString();
                    custOrder.orderNum2 = reader["FISNO2"].ToString();
                    custOrder.chosen = Convert.ToBoolean(reader["ISCHOSEN"]);
                    custOrder.priority = Convert.ToInt32(reader["PRIOR"]);
                    custOrder.exp1 = reader["BELGENOTU"].ToString();
                    custOrder.kalanKg = Convert.ToDouble(reader["KALANKG"]);
                    custOrder.kalanPk = Convert.ToDouble(reader["KALANPK"]);
                    custOrders.Add(custOrder);
                }
                await reader.CloseAsync();
                return custOrders;
            }
            catch (Exception ex)
            {
                return custOrders;
            }
            finally
            {
                await CloseSql();
            }
        }



        public async Task<List<AddStockReserveDTO>> GetStockReserves()
        {
            List<AddStockReserveDTO> stockReserves = new List<AddStockReserveDTO>();
            try
            {
                _connection.ConnectionString = _configuration.GetConnectionString("sqlConnection");
                await OpenSql();
                _connection.ChangeDatabase("GURMENPVC2021");
                SqlCommand cmd = new SqlCommand(@"  SELECT [code],[orderNum],[quantity1],[usedCode] FROM [BarcodeApp].[dbo].[StockReserves]
  where code+orderNum IN(SELECT STOK_KODU+FISNO FROM GRM_PLSIPARIS WHERE GRUP_KODU='03')", _connection);
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
            List<Caliber> calibers = new List<Caliber>();
            try
            {
                _connection.ConnectionString = _configuration.GetConnectionString("sqlConnection");
                await OpenSql();
                _connection.ChangeDatabase("GURMENPVC2021");
                SqlCommand cmd = new SqlCommand(@"select KOD,AD,STHIZ,KATALOG_KODU,MAXAGIR,ANA_YARDIMCI FROM EBAKALIPLAR 
WHERE MAXAGIR IS NOT NULL
", _connection);
                cmd.Parameters.Clear();
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    Caliber caliber = new Caliber();
                    caliber.code = reader["KOD"].ToString();
                    caliber.name = reader["AD"].ToString();
                    caliber.speed = Convert.ToDouble(reader["STHIZ"]);
                    caliber.catalogCode = reader["KATALOG_KODU"].ToString();
                    caliber.weight = Convert.ToDouble(reader["MAXAGIR"]);
                    caliber.prType = reader["ANA_YARDIMCI"].ToString();
                    caliber.isAbleTo = new List<string>();
                    calibers.Add(caliber);
                }
                await reader.CloseAsync();
                foreach (var cal in calibers)
                {
                    cmd.CommandText = "select MAKINE FROM FLOW_KALIPMAKINE WHERE KALIPKODU=@KALIP";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@KALIP", cal.code);
                    reader = await cmd.ExecuteReaderAsync();
                    while(await reader.ReadAsync())
                    {
                        cal.isAbleTo.Add(reader["MAKINE"].ToString());
                    }
                    await reader.CloseAsync();
                }
                await CloseSql();
                return calibers;
            }
            catch (Exception ex)
            {
                return calibers;
            }
            finally
            {
                await CloseSql();
            }
        }

        public async Task<Caliber> GetCaliber(string code)
        {
            Caliber caliber = new Caliber();
            try
            {
                _connection.ConnectionString = _configuration.GetConnectionString("sqlConnection");
                await OpenSql();
                _connection.ChangeDatabase("GURMENPVC2021");
                SqlCommand cmd = new SqlCommand(@"select KOD,AD,STHIZ,KATALOG_KODU,MAXAGIR,ANA_YARDIMCI FROM EBAKALIPLAR 
WHERE MAXAGIR IS NOT NULL and KOD=@KOD
", _connection);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@KOD",code);
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    caliber.code = reader["KOD"].ToString();
                    caliber.name = reader["AD"].ToString();
                    caliber.speed = Convert.ToDouble(reader["STHIZ"]);
                    caliber.catalogCode = reader["KATALOG_KODU"].ToString();
                    caliber.weight = Convert.ToDouble(reader["MAXAGIR"]);
                    caliber.prType = reader["ANA_YARDIMCI"].ToString();
                }
                await reader.CloseAsync();
                await CloseSql();
                return caliber;
            }
            catch (Exception ex)
            {
                return caliber;
            }
            finally
            {
                await CloseSql();
            }
        }

        public async Task<List<CustomerOrderDetail>> GetCOrderDetails(string orderNum)
        {
            List<CustomerOrderDetail> customerOrderDetails = new List<CustomerOrderDetail>();
            try
            {
                _connection.ConnectionString = _configuration.GetConnectionString("sqlConnection");
                await OpenSql();
                _connection.ChangeDatabase("GURMENPVC2021");
                SqlCommand cmd = new SqlCommand(@"SELECT ISNULL((SELECT STOK_ADI FROM TBLSTSABIT WHERE STOK_KODU=YMMKODU),'YMM BULUNAMADI')YMMADI,* FROM(
SELECT STOK_KOD,STOK_ADI,GURMENFISNO,BR1,SUM(KALAN_MIKTAR)KALAN_MIKTAR1,BR2,SUM(SIPKALAN_MIKTAR)KALAN_MIKTAR2,ACIKLAMA1,
BR3,SUM(KALANKG) KALAN_MIKTAR3,
ISNULL((SELECT SUM(R.QUANTITY1) FROM BARCODEAPP..STOCKRESERVES R  WHERE CODE+ORDERNUM=STOK_KOD+GURMENFISNO),0)HAZIRMIKTAR,
ISNULL((SELECT TOP 1 DEGER1 FROM FLOW_STSABIT WHERE DEGERTIPI='HAMUR RENK' AND KOSUL=SUBSTRING(STOK_KOD,12,2)),'Beyaz')HAMUR,
ISNULL((SELECT TOP 1 HAM_KODU FROM TBLSTOKURM WHERE MAMUL_KODU=STOK_KOD AND HAM_KODU LIKE 'YMM%'),'')YMMKODU
FROM (                   
  SELECT URETICI_KODU, STOK_KODU  STOK_KOD,        
  STOK_ADI  STOK_ADI,        
 FISNO GURMENFISNO,BR2,X.KALAN_MIKTAR,BR1,BR3,X.KALAN_MIKTAR*CEVRIM2 SIPKALAN_MIKTAR,0 KULL_BAKIYE,0 ACIKISEMRI,X.KALAN_MIKTAR*CEVRIM3 KALANKG,                  
 0 REZSTOK,0 REZISMR,0 PLANMIKTAR,RECETE,'Uygun'UYGUN,ISNULL(x.ACIKLAMA,'-')+' '+UNVAN ACIKLAMA1,SIRANO ID
 FROM(         
 SELECT S.*,CASE WHEN (SELECT COUNT(*) FROM TBLSTOKURM R WHERE R.MAMUL_KODU=S.STOK_KODU)>0              
 THEN 'VAR' else 'YOK' END RECETE 
 FROM GRM_PLSIPARIS   S  
 WHERE S.GRUP_KODU IN('02','03') AND FISNO=@ORDERNUM
 )X     
 WHERE KALAN_MIKTAR>0       
)Y  
GROUP BY  STOK_KOD,STOK_ADI,GURMENFISNO,BR2,KULL_BAKIYE,ACIKLAMA1,BR1,BR3
)X
", _connection);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@ORDERNUM", orderNum);
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    CustomerOrderDetail cODetail = new CustomerOrderDetail();
                    cODetail.code = reader["STOK_KOD"].ToString();
                    cODetail.name = reader["STOK_ADI"].ToString();
                    cODetail.orderNum = reader["GURMENFISNO"].ToString();
                    cODetail.exp1 = reader["ACIKLAMA1"].ToString();
                    cODetail.unit1 = reader["BR1"].ToString();
                    cODetail.unit2 = reader["BR2"].ToString();
                    cODetail.unit3 = reader["BR3"].ToString();
                    cODetail.subCode = reader["YMMKODU"].ToString();
                    cODetail.dColor = reader["HAMUR"].ToString();
                    cODetail.readyQuantity = Convert.ToDouble(reader["HAZIRMIKTAR"]);
                    cODetail.quantity1 = Convert.ToDouble(reader["KALAN_MIKTAR1"]);
                    cODetail.quantity2 = Convert.ToDouble(reader["KALAN_MIKTAR2"]);
                    cODetail.quantity3 = Convert.ToDouble(reader["KALAN_MIKTAR3"]);
                    cODetail.subName= reader["YMMADI"].ToString();
                    customerOrderDetails.Add(cODetail);
                }
                await reader.CloseAsync();
                await CloseSql();
                return customerOrderDetails;
            }
            catch (Exception ex)
            {
                return customerOrderDetails;
            }
            finally
            {
                await CloseSql();

            }
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
I.[MIKTAR],[ACIK1],[ACIK2],[ACIK3],[ACIK4],[ACIK5],ISNULL((SELECT SUM(P.quantity) FROM BarcodeApp..Productions P WHERE P.orderId=I.ID and P.isOkay=1),0) URETILEN,CHOSEN
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
                _connection.ChangeDatabase("GURMENPVC2021");

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

    }
}
