using System.ComponentModel.DataAnnotations;

namespace JwToken.Models.Entities
{
    /// <summary>
    /// User DB Model
    /// </summary>
    public class UserModel
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
