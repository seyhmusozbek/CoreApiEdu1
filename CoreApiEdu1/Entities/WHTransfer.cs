using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApiEdu1.Entities
{
    public class WHTransfer
    {
        public int id { get; set; }
        [MaxLength(100)]
        public string stokKodu { get; set; }
        public int cikDepo { get; set; }
        public int girDepo { get; set; }
        public double miktar { get; set; }
        [MaxLength(200)]
        public string aciklama { get; set; }
        [MaxLength(50)]
        public string aciklama2 { get; set; }
        public int obr { get; set; }
        public DateTime date { get; set; }
        public bool isSaved { get; set; }
        public bool isFaulty { get; set; }
    }
}
