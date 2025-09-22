using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TaxiApp.Interfaces;
using TaxiApp.Services;

namespace TaxiApp.Models
{
    public class DefaultData
    {
        public static async Task AddAdmin(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<IUserService>();

            string username = configuration.GetSection("AdminData")["Username"];
            string password = configuration.GetSection("AdminData")["Password"];

            bool valid = await userManager.AddAdmin(username, password);
        }
    }
}
