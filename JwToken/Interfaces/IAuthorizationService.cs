using JwToken.Models.Authentication;
using JwToken.Models.Entities;

namespace JwToken.Interfaces
{
    /// <summary>
    /// AuthorizationService
    /// </summary>
    public interface IAuthorizationService
    {
        string GenerateJwToken(UserModel user);
        AuthModel GenerateTokens(UserModel user);
        AuthModel RefreshTokens(string token, string refreshToken);
    }
}
