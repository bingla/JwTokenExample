using JwToken.Context;
using JwToken.Interfaces;
using JwToken.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JwToken.Services
{
    /// <summary>
    /// UserService
    /// </summary>
    public class UserService : IUserService
    {
        private readonly ApiContext _context;
        
        public UserService(
            ApiContext context)
        {
            _context = context;
        }

        public UserModel GetUser(int userId)
        {
            return _context.Users.Find(userId);
        }

        public UserModel GetUser(string userEmail)
        {
            return _context.Users.FirstOrDefault(u => u.Email.Equals(userEmail, StringComparison.InvariantCultureIgnoreCase));
        }

        public IEnumerable<UserModel> GetUsers()
        {
            return _context.Users.ToList();
        }

        public void UpdateRefreshToken(int userId, string refreshToken, DateTime expirationDate)
        {
            var user = GetUser(userId);
            if (user != null)
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpirationDate = expirationDate;
                _context.SaveChanges();
            }
        }
    }
}
