using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApiEdu1.ADONet.AdoEnts
{
    public class BarcodeInfo
    {
        public int barcodeId { get; set; }
        public string stokKodu { get; set; }
        public string stokAdi { get; set; }
        public int depo { get; set; }
        public double bakiye1 { get; set; }
        public string olcuBr1 { get; set; }
        public double bakiye2 { get; set; }
        public double kullanilan1 { get; set; }
        public double pay2 { get; set; }
        public double payda1 { get; set; }
        public string olcuBr2 { get; set; }
        public string group { get; set; }
        public DateTime basimTarihi { get; set; }
        public bool isColored { get; set; }
        public string length { get; set; }
        public string catalogCode { get; set; }
        public string protectiveFoil { get; set; }
        public string extColorCode { get; set; }
        public string extColorFace { get; set; }
        public string gasketColor { get; set; }
        public string gasketType { get; set; }
        public string wallGasket { get; set; }

    }
}
