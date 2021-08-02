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
    }
}
