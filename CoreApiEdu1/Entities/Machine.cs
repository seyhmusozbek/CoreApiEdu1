using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApiEdu1.Entities
{
    public class Machine
    {
        public int id { get; set; }
        public string machineName{ get; set; }
        public string machineGroup { get; set; }
        public bool currentStatus { get; set; }
        public int currentOrder { get; set; }
    }
}
