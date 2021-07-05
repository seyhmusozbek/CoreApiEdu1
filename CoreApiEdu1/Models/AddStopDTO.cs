using CoreApiEdu1.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApiEdu1.Models
{
    public class AddStopDTO
    {
        public Machine machine { get; set; }
        public DateTime date { get; set; }
        public bool isStop { get; set; }
        [MaxLength(100)]
        public string exp1 { get; set; }
    }
}
