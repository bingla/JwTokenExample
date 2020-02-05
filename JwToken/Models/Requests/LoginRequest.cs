using System.ComponentModel.DataAnnotations;

namespace JwToken.Models.Requests
{
    /// <summary>
    /// Login Request
    /// </summary>
    public class LoginRequest
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
