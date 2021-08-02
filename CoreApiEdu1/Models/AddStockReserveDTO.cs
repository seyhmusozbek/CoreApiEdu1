using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApiEdu1.Models
{
    public class AddStockReserveDTO
    {
        [MaxLength(100)]
        public string code { get; set; }
        [MaxLength(100)]
        public string orderNum { get; set; }
        [MaxLength(100)]
        public string usedCode { get; set; }
        public double quantity1 { get; set; }

    }
}
