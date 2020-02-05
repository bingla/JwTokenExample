using JwToken.Models.Entities;

namespace JwToken.Interfaces
{
    /// <summary>
    /// AuthorizationService
    /// </summary>
    public interface IAuthorizationService
    {
        string GenerateJwToken(UserModel user);
    }
}
