using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApiEdu1.Entities
{
    public class ChosenOrder
    {
        public int id { get; set; }
        [MaxLength(100)]
        public string orderNum { get; set; }
        public DateTime finishTime { get; set; }
        public int priority { get; set; }
        public string merge { get; set; }
    }
}
