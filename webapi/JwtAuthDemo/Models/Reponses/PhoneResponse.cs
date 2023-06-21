using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtAuthDemo.Models.Reponses
{
    public class PhoneResponse
    {
        public int total { get; set; }
        public Phone[] items { get; set; }
    }
}
