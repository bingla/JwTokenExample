using JwToken.Interfaces;
using JwToken.Models.Authentication;
using JwToken.Models.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace JwToken.Services
{
    /// <summary>
    /// AuthorizationService
    /// Use JWToken to authorize against authenticated users
    /// </summary>
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IConfiguration _config;
        private readonly IUserService _userService;
        private readonly IJWTokenGenerator _jwTokenGenerator;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private string _jwtSecret = string.Empty;
        private int _refreshTokenExpirationLengthInDays = 1;

        public AuthorizationService(
            IConfiguration config,
            IUserService userService,
            IJWTokenGenerator jwTokenGenerator,
            IRefreshTokenGenerator refreshTokenGenerator)
        {
            _config = config;
            _userService = userService;
            _jwTokenGenerator = jwTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;

            _jwtSecret = _config.GetValue<string>("Auth:JwTSecret");
            _refreshTokenExpirationLengthInDays = int.Parse(_config.GetValue<string>("Auth:RefreshTokenLifetimeInDays"));
        }

        public string GenerateJwToken(UserModel user)
        {
            return _jwTokenGenerator.Generate(_jwtSecret, user.Id, user.Email);
        }

        public AuthModel GenerateTokens(UserModel user)
        {
            var token = _jwTokenGenerator.Generate(_jwtSecret, user.Id, user.Email);
            var refreshToken = _refreshTokenGenerator.Generate();
            var refreshTokenExpirationDate = GenerateRefreshTokenExpirationDate();

            _userService.UpdateRefreshToken(user.Id, refreshToken, refreshTokenExpirationDate);

            return new AuthModel
            {
                UserId = user.Id,
                Token = token,
                Refresh = refreshToken
            };
        }

        public AuthModel RefreshTokens(string token, string refreshToken)
        {
            var tokenClaims = GetClaimsFromJwToken(_jwtSecret, token);
            var userNameIdentifier = tokenClaims.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.NameIdentifier));

            if (userNameIdentifier == null)
                throw new ArgumentOutOfRangeException("NameIdentifier was null");

            var user = _userService.GetUser(int.Parse(userNameIdentifier.Value));
            if (user == null)
                throw new ArgumentNullException("User not found");

            if (!user.RefreshToken.Equals(refreshToken))
                throw new ArgumentOutOfRangeException("RefreshToken mismatch");

            if (!user.RefreshTokenExpirationDate.HasValue && user.RefreshTokenExpirationDate.Value > DateTime.Now)
                throw new ArgumentOutOfRangeException("RefreshToken out of date");

            var newToken = _jwTokenGenerator.Generate(_jwtSecret, tokenClaims);
            var newRefreshToken = _refreshTokenGenerator.Generate();
            var newRefreshTokenExpirationDate = GenerateRefreshTokenExpirationDate();

            _userService.UpdateRefreshToken(user.Id, refreshToken, newRefreshTokenExpirationDate);

            return new AuthModel
            {
                UserId = user.Id,
                Token = newToken,
                Refresh = newRefreshToken
            };
        }

        private DateTime GenerateRefreshTokenExpirationDate()
        {
            return DateTime.Now.AddDays(_refreshTokenExpirationLengthInDays);
        }

        private IEnumerable<Claim> GetClaimsFromJwToken(string secret, string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);

            var jwToken = validatedToken as JwtSecurityToken;
            if (jwToken == null || !jwToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token passed");
            }

            return principal.Claims;
        }
    }
}
