using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApiEdu1.ADONet.AdoEnts
{
    public class Caliber
    {
        public int Id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public double speed { get; set; }
        public string catalogCode { get; set; }
        public double weight { get; set; }
        public int count { get; set; }
        public string prType { get; set; }
        public bool chosen { get; set; } = false;
        public List<MachineCaliberMappingDetails> isAbleTo { get; set; }
    }

    public class MachineCaliberMappingDetails
    {
        public string machineName { get; set; }
        public double speed { get; set; }
    }
}
