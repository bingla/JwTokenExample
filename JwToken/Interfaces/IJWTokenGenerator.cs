using System.Collections.Generic;
using System.Security.Claims;

namespace JwToken.Interfaces
{
    public interface IJWTokenGenerator
    {
        string Generate(string secret, int userId, string userEmail, string userRole);
        string Generate(string secret, IEnumerable<Claim> claims);
    }
}
