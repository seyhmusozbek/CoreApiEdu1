using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApiEdu1.Entities
{
    public class CountMaster
    {
        public int id { get; set; }
        public DateTime date { get; set; }
        public string exp { get; set; }
        public int warehouse { get; set; }
        [MaxLength(100)]
        public string userName { get; set; }
        public ICollection<CountDetail> details { get; set; }
        public bool isActive { get; set; }
    }
}
