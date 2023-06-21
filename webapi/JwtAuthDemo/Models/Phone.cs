using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JwtAuthDemo.Models
{
    public class Phone
    {
        public Guid ID { get; set; }
        public string PhoneNumber { get; set; }
        public string IP { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public bool ConnectStatus { get; set; }


        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
