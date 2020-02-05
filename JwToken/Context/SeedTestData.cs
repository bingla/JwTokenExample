using JwToken.Models.Entities;
using System.Collections.Generic;

namespace JwToken.Context
{
    /// <summary>
    /// Seed Test Data
    /// Seed the context with data
    /// </summary>
    public static class SeedTestData
    {
        public static void SeedUsers(ApiContext context)
        {
            var users = new List<UserModel>
            {
                new UserModel
                {
                    FirstName = "Luke",
                    LastName = "Skywalker",
                    Email = "luke.skywalker@skywalker-ranch.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("skywalker")
                },
                new UserModel
                {
                    FirstName = "Han",
                    LastName = "Solo",
                    Email = "han.solo@kessel-runs.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("solo")
                }
            };
            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}
