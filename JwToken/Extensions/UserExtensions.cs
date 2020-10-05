using JwToken.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

namespace JwToken.Extensions
{
    /// <summary>
    /// User Extensions
    /// </summary>
    public static class UserExtensions
    {
        /// <summary>
        /// Hide PasswordHash on Users by setting value to null
        /// </summary>
        /// <param name="users">List of users</param>
        public static IEnumerable<UserModel> WithoutPasswordHashes(this IEnumerable<UserModel> users)
        {
            return users.Select(WithoutPasswordHash);
        }

        /// <summary>
        /// Hide PasswordHash on User by setting value to null
        /// </summary>
        /// <param name="user">User</param>
        public static UserModel WithoutPasswordHash(this UserModel user)
        {
            user.PasswordHash = null;
            return user;
        }

        /// <summary>
        /// Hide RefreshTokens on Users by setting value to null
        /// </summary>
        /// <param name="users">List of users</param>
        public static IEnumerable<UserModel> WithoutRefreshTokenInfos(this IEnumerable<UserModel> users)
        {
            return users.Select(WithoutRefreshTokenInfo);
        }

        /// <summary>
        /// Hide RefreshToken on User by setting value to null
        /// </summary>
        /// <param name="user">User</param>
        public static UserModel WithoutRefreshTokenInfo(this UserModel user)
        {
            user.RefreshToken = string.Empty;
            user.RefreshTokenExpirationDate = null;
            return user;
        }
    }
}
