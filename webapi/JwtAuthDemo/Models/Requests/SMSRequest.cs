using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtAuthDemo.Models.Requests
{
    public class SMSRequest
    {
        public int pageIndex { get; set; } = 1;
        public int pageSize { get; set; } = 10;
        public string? from { get; set; }
        public int? index { get; set; }
        public string? content { get; set; }
        public DateTime? minReceivedTime { get; set; } = DateTime.MinValue;
        public DateTime? maxReceivedTime { get; set; } = DateTime.MaxValue;
        public Guid? phoneID { get; set; }
        public string? phoneNumber { get; set; }
    }
}
