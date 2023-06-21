using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtAuthDemo.Models.Reponses
{
    public class SMSResponse
    {
        public int total { get; set; }
        public SMSMod[] items { get; set; }
    }
    public class SMSMod : SMS
    {
        public string PhoneNumber { get; set; }
    }
}
