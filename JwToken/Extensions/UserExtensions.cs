using JwToken.Models.Entities;
using System.Collections.Generic;
using System.Linq;

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
        /// <param name="users">Users</param>
        public static UserModel WithoutPasswordHash(this UserModel user)
        {
            user.PasswordHash = null;
            return user;
        }
    }
}
