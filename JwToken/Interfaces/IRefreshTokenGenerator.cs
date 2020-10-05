using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwToken.Interfaces
{
    public interface IRefreshTokenGenerator
    {
        string Generate();
    }
}
