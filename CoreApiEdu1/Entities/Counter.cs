using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApiEdu1.Entities
{
    public class Counter
    {
        public int id { get; set; }
        public string name { get; set; }
        public int lastValue { get; set; }
        public DateTime lastDate { get; set; }
    }
}
