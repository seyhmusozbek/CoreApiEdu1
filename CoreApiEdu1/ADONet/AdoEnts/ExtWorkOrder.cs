using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApiEdu1.ADONet.AdoEnts
{
    public class ExtWorkOrder
    {
        public string WorkNum { get; set; }
        public string ProductCode { get; set; }
        public string Machine { get; set; }
        public string ProductName { get; set; }
        public DateTime StartDate { get; set; }
        public int Priority { get; set; }
        public int Id { get; set; }
        public string Exp1 { get; set; }
        public string Exp2 { get; set; }
        public string Exp3 { get; set; }
        public string Exp4 { get; set; }
        public string Exp5 { get; set; }
        public double Planned { get; set; }
        public double Produced { get; set; }
    }
}
