using CoreApiEdu1.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApiEdu1.Models
{
    public class CountMasterDTO
    {
        public DateTime date { get; set; }
        public string exp { get; set; }
        public int warehouse { get; set; }
        [MaxLength(100)]
        public string userName { get; set; }
        public ICollection<CountDetailsDTO> details { get; set; }
    }
}
