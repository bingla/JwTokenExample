using JwToken.Models.Entities;
using System;
using System.Collections.Generic;

namespace JwToken.Interfaces
{
    /// <summary>
    /// UserService
    /// </summary>
    public interface IUserService
    {
        UserModel GetUser(int userId);
        UserModel GetUser(string userEmail);
        IEnumerable<UserModel> GetUsers();
        void UpdateRefreshToken(int userId, string refreshToken, DateTime expirationDate);
    }
}
