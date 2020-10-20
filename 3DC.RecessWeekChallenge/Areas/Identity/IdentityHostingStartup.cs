using System;
using _3DC.RecessWeekChallenge.Areas.Identity.Data;
using _3DC.RecessWeekChallenge.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(_3DC.RecessWeekChallenge.Areas.Identity.IdentityHostingStartup))]
namespace _3DC.RecessWeekChallenge.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {

                var builder = new SqlConnectionStringBuilder(
                    context.Configuration.GetConnectionString("LoginContextConnection"));
                builder.Password = context.Configuration["DbSvPw"];
                string _connection = builder.ConnectionString;

                services.AddDbContext<LoginContext>(options =>
                {
                    options.UseSqlServer(_connection);
                });

                services.AddDefaultIdentity<LoginUser>(options => options.SignIn.RequireConfirmedAccount = false)
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<LoginContext>();

                services.AddAuthorization(options =>
                {
                    options.AddPolicy("AdminPolicy", policy =>
                        policy.RequireAssertion(context =>
                            context.User.IsInRole("Administrator")));
                });

            });
        }
    }
}