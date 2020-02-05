using JwToken.Context;
using JwToken.Extensions;
using JwToken.Interfaces;
using JwToken.Models.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;

namespace JwToken.Services
{
    /// <summary>
    /// AuthenticationService
    /// Handle User Authentication and Claims for the currently authenticated and authorized user
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApiContext _context;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContentAccessor;

        public AuthenticationService(
            ApiContext context,
            IUserService userService,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userService = userService;
            _httpContentAccessor = httpContextAccessor;
        }

        public bool ValidateUserPassword(string login, string password, out UserModel user)
        {
            user = ValidateUserPassword(login, password);
            if (user == null)
                return false;

            return true;
        }

        public UserModel ValidateUserPassword(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                return null;

            var user = _userService.GetUser(login);
            if (user == null)
                return null;

            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash) 
                    ? user.WithoutPasswordHash()
                    : null;
        }

        public UserModel SetUserPassword(string login, string newPassword)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(newPassword))
                return null;

            var user = _context.Users.FirstOrDefault(u => u.Email.Equals(login, StringComparison.InvariantCultureIgnoreCase));
            if (user == null)
                return null;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            _context.SaveChanges();

            return user.WithoutPasswordHash();
        }

        public UserModel GetAuthorizedUser()
        {
            var userId = GetAuthorizedUserId();
            return _userService.GetUser(userId).WithoutPasswordHash();
        }

        public ClaimsPrincipal GetAuthorizedUserClaimsPrincipal()
        {
            return _httpContentAccessor.HttpContext.User;
        }

        public Claim GetAuthorizedUserClaim(string claimType)
        {
            var claimsPrincipal = GetAuthorizedUserClaimsPrincipal();
            if (claimsPrincipal == null)
                throw new ArgumentException();

            return claimsPrincipal.FindFirst(claimType);
        }

        public int GetAuthorizedUserId()
        {
            var userClaim = GetAuthorizedUserClaim(ClaimTypes.NameIdentifier);
            if (userClaim == null)
                throw new ArgumentOutOfRangeException();

            if (!int.TryParse(userClaim.Value, out var userId))
                throw new FormatException();

            return Convert.ToInt32(userId);
        }
    }
}
