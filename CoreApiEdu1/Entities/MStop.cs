using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApiEdu1.Entities
{
    public class MStop
    {
        public int id { get; set; }
        public Machine machine { get; set; }
        public DateTime start { get; set; }
        public DateTime finish { get; set; }
        public int minutes { get; set; }
    }
}
