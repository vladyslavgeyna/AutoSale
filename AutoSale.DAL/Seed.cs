using System.Text.RegularExpressions;
using AutoSale.Domain.Enum;
using AutoSale.Domain.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace AutoSale.DAL
{
    public static class Seed
    {
        public static async Task<IApplicationBuilder> SeedData(this IApplicationBuilder applicationBuilder)
        {
            using (var scope = applicationBuilder.ApplicationServices.CreateScope())
            {
                using (var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>())
                {
                    try
                    {
                        if (!await roleManager.RoleExistsAsync(Role.Admin.ToString()))
                        {
                            await roleManager.CreateAsync(new IdentityRole(Role.Admin.ToString()));
                        }
                        
                        if (!await roleManager.RoleExistsAsync(Role.User.ToString()))
                        {
                            await roleManager.CreateAsync(new IdentityRole(Role.User.ToString()));
                        }
                    }
                    catch
                    {
                        return applicationBuilder;
                    }    
                }
                return applicationBuilder;
            }
        }
    }


}