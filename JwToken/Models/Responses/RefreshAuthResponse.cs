using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwToken.Models.Responses
{
    public class RefreshAuthResponse
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public string Refresh { get; set; }
    }
}
