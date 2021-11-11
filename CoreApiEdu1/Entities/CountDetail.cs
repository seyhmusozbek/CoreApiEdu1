using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApiEdu1.Entities
{
    public class CountDetail
    {
        public int id { get; set; }
        [MaxLength(100)]
        public string stokKodu { get; set; }
        public double miktar { get; set; }
        [MaxLength(200)]
        public string aciklama { get; set; }
        public int obr { get; set; }
        public int countMasterId { get; set; }
        [MaxLength(150)]
        public string stokAdi { get; set; }
        [MaxLength(15)]
        public string obrStr { get; set; }
        public CountMaster countMaster { get; set; }
    }
}
