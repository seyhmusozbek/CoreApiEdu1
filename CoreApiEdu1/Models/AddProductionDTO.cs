using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApiEdu1.Models
{
    public class AddProductionDTO
    {
        public int orderId { get; set; }
        public double quantity { get; set; }
        public bool isOkay { get; set; }
        [MaxLength(5)]
        public string shiftChar { get; set; }
        [MaxLength(50)]
        public string shiftHour { get; set; }
        public string exp1 { get; set; }

    }
}
