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
        public static void SeedData(ApiContext context)
        {
            var users = new List<UserModel>
            {
                new UserModel
                {
                    FirstName = "R2D2",
                    LastName = "",
                    Email = "R2D2@droid-rebellion.com",
                    Role = Roles.Admin,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("C3PO")
                },
                new UserModel
                {
                    FirstName = "Luke",
                    LastName = "Skywalker",
                    Email = "luke.skywalker@skywalker-ranch.com",
                    Role = Roles.User,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("skywalker")
                },
                new UserModel
                {
                    FirstName = "Han",
                    LastName = "Solo",
                    Email = "han.solo@kessel-runs.com",
                    Role = Roles.User,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("solo")
                }
            };
            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}
