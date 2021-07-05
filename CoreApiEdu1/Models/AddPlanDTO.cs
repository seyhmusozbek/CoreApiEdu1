using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApiEdu1.Models
{
    public class AddPlanDTO
    {
        [MaxLength(10)]
        public string machine { get; set; }
        [MaxLength(10)]
        public string caliber { get; set; }
        public DateTime finishTime { get; set; }
        public double hour { get; set; }
        public double quantity1 { get; set; }
        public double readyQuantity { get; set; }
        [MaxLength(50)]
        public string orderNum { get; set; }
        [MaxLength(150)]
        public string customerName { get; set; }
        [MaxLength(50)]
        public string code { get; set; }
    }
}
