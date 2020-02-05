
using Microsoft.EntityFrameworkCore;
using JwToken.Models.Entities;

namespace JwToken.Context
{
    /// <summary>
    /// Api Context
    /// </summary>
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; }
    }
}
