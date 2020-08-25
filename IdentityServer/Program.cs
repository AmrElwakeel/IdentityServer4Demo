using IdentityServer.Configurations;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;

namespace IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            //Create Users InMEmory

            using (var scope = host.Services.CreateScope())
            {
                var userManger = scope.ServiceProvider
                    .GetRequiredService<UserManager<IdentityUser>>();

                //var roleManger = scope.ServiceProvider
                //    .GetRequiredService<RoleManager<IdentityRole>>();

                //Create Users
                var user1 = new IdentityUser { UserName = "amr" };
                var user2 = new IdentityUser { UserName = "eslam" };

                userManger.CreateAsync(user1, "amrdemo").GetAwaiter().GetResult();
                userManger.CreateAsync(user2, "eslamdemo").GetAwaiter().GetResult();

                ////Craete Role

                //var valuesAPIUser = new IdentityRole { Name = "valuesAPIUser" };
                //roleManger.CreateAsync(valuesAPIUser);

                scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>()
                .Database.Migrate();

                var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                if (!context.Clients.Any())
                {
                    foreach (var client in Config.Clients)
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in Config.IdentityResources)
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiScopes.Any())
                {
                    foreach (var resource in Config.ApiScopes)
                    {
                        context.ApiScopes.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }


            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
