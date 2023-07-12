using API.Role;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Infrastructure.Data.IdentityDB
{
    public static class IdentityDBContextSeed
    {
        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var result = await roleManager.RoleExistsAsync(UserRoles.Admin);
                result = await roleManager.RoleExistsAsync(UserRoles.Employe);
                result = await roleManager.RoleExistsAsync(UserRoles.Customer);

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.Employe))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Employe));
                if (!await roleManager.RoleExistsAsync(UserRoles.Customer))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Customer));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();


                

                var admin = await userManager.FindByEmailAsync("admin@store.com");
                if (admin == null)
                {
                    var newAdmin = new User()
                    {
                        UserName = "admin@store.com",
                        DisplayName = "Admin",
                        Email = "admin@store.com"
                    };

                    var result1 = await userManager.CreateAsync(newAdmin, "Coding@1234?");
                    result1 = await userManager.AddToRoleAsync(newAdmin, UserRoles.Admin);
                }


                var employe = await userManager.FindByEmailAsync("employe@store.com");
                if (employe == null)
                {
                    var newEmploye = new User()
                    {
                        UserName = "employe@store.com",
                        DisplayName = "Employe",
                        Email = "employe@store.com"
                    };
                    await userManager.CreateAsync(newEmploye, "Coding@1234?");
                    await userManager.AddToRoleAsync(newEmploye, UserRoles.Employe);
                }

                var customer = await userManager.FindByEmailAsync("customer@store.com");
                if (customer == null)
                {
                    var newCustomer = new User()
                    {
                        UserName = "customer@store.com",
                        DisplayName = "customer",
                        Email = "customer@store.com"
                    };
                    await userManager.CreateAsync(newCustomer, "Coding@1234?");
                    await userManager.AddToRoleAsync(newCustomer, UserRoles.Customer);
                }

                var customer2 = await userManager.FindByEmailAsync("mixa@store.com");
                if (customer2 == null)
                {
                    var newCustomer = new User()
                    {
                        UserName = "mixa@store.com",
                        DisplayName = "mixa",
                        Email = "mixa@store.com"
                    };
                    await userManager.CreateAsync(newCustomer, "Coding@1234?");
                    await userManager.AddToRoleAsync(newCustomer, UserRoles.Customer);
                }
            }
        }
    }
}
