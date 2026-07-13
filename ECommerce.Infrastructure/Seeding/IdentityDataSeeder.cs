using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.Identity;
using ECommerce.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.Infrastructure.Seeding
{
    public class IdentityDataSeeder : IDataSeeder
    {
        private readonly StoreIdentityDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger<IdentityDataSeeder> logger;

        public IdentityDataSeeder(StoreIdentityDbContext dbContext, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, ILogger<IdentityDataSeeder> logger)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.logger = logger;
        }
        public async Task SeedAsync(CancellationToken ct = default)
        {
            try
            {
                var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync(ct);
                if (pendingMigrations.Any())
                {
                    await dbContext.Database.MigrateAsync(ct);
                }

                if (!await roleManager.Roles.AnyAsync(ct))
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                    await roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }

                if (!await userManager.Users.AnyAsync(ct))
                {
                    var adminUser = new ApplicationUser
                    {
                        DisplayName = "Islam Omar",
                        UserName = "Islam",
                        Email = "islam3marahmed@gmail.com",
                        PhoneNumber = "01036272141",
                    };
                    var Result = await userManager.CreateAsync(adminUser, "P@ssw0rd");

                    if (Result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                    else
                    {
                        logger.LogWarning("The User Did Not Create");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Can Not Seed Identity Data");
                throw;
            }
        }
    }
}
