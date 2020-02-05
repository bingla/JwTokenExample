using System.ComponentModel.DataAnnotations;

namespace JwToken.Models.Requests
{
    /// <summary>
    /// Set new Password Request
    /// </summary>
    public class SetUserPasswordRequest
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
