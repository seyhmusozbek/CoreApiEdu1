using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApiEdu1.ADONet.AdoEnts
{
    public class CustomerOrder
    {
        public int id { get; set; }
        public string orderNum { get; set; }
        public string orderNum2 { get; set; }
        public string customerName{ get; set; }
        public bool chosen { get; set; }
        public int priority { get; set; }
        public string exp1 { get; set; }
        public double kalanKg { get; set; }
        public double kalanPk { get; set; }

    }
}
