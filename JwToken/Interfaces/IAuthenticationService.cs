using JwToken.Models.Entities;
using System.Security.Claims;

namespace JwToken.Interfaces
{
    /// <summary>
    /// AuthenticationService
    /// </summary>
    public interface IAuthenticationService
    {
        bool ValidateUserPassword(string login, string password, out UserModel user);
        UserModel ValidateUserPassword(string login, string password);
        UserModel SetUserPassword(string login, string newPassword);
        UserModel GetAuthorizedUser();

        ClaimsPrincipal GetAuthorizedUserClaimsPrincipal();
        Claim GetAuthorizedUserClaim(string claim);
        public int GetAuthorizedUserId();
    }
}
