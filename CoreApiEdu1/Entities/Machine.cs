using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApiEdu1.Entities
{
    public class Machine
    {
        public int id { get; set; }
        [MaxLength(100)]
        public string machineName{ get; set; }
        public string machineGroup { get; set; }
        public bool currentStatus { get; set; }
        public int currentOrder { get; set; }
        public string currentCaliber { get; set; }
        [MaxLength(10)]
        public string colorFilter { get; set; }
        public DateTime? lastStopped { get; set; } = DateTime.Now;
    }
}
