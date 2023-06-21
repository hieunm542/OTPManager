using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtAuthDemo.Models
{
    public class SMS
    {
        public Guid ID { get; set; }
        public int Index { get; set; }
        public string From { get; set; }
        public string Content { get; set; }
        public DateTime ReceivedTime { get; set; }
        public Guid PhoneID { get; set; }

        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
