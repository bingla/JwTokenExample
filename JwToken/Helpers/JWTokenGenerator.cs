using JwToken.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwToken.Helpers
{
    public class JWTokenGenerator : IJWTokenGenerator
    {
        private int _jwtLifetimeInMinutes = 1;

        public JWTokenGenerator(IConfiguration config)
        {
            _jwtLifetimeInMinutes = int.Parse(config.GetValue<string>("Auth:JwTLifetimeInMinutes"));
        }

        public string Generate(string secret, int userId, string userEmail)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, userEmail),
            };

            return Generate(secret, claims);
        }

        public string Generate(string secret, IEnumerable<Claim> claims)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtLifetimeInMinutes),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                        SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
