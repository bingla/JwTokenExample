using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JwToken.Models.Requests
{
    public class RefreshAuthRequest
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string Refresh { get; set; }
    }
}
