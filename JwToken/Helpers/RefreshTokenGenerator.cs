using JwToken.Interfaces;
using System;
using System.Security.Cryptography;

namespace JwToken.Helpers
{
    public class RefreshTokenGenerator : IRefreshTokenGenerator
    {
        public string Generate()
        {
            var token = new byte[32];
            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(token);
                return Convert.ToBase64String(token);
            }
        }
    }
}
