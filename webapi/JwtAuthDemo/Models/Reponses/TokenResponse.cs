using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtAuthDemo.Models.Reponses
{
    public class TokenResponse
    {
        public int total { get; set; }
        public Token[] items { get; set; }
    }
}
