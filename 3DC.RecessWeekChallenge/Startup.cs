﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using _3DC.RecessWeekChallenge.Models;
using _3DC.RecessWeekChallenge.Data;
using Microsoft.Data.SqlClient;
using _3DC.RecessWeekChallenge.Services;

namespace _3DC.RecessWeekChallenge
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddHttpClient();
            services.AddHostedService<HackerrankLeaderboardService>();

            string _connection;
            if (_env.IsProduction())
            {
                var builder = new SqlConnectionStringBuilder(
                    Configuration.GetConnectionString("_3DCRecessWeekChallengeContext"));
                builder.Password = Configuration["DbSvPw"];
                _connection = builder.ConnectionString;
            } else
            {
                var builder = new SqlConnectionStringBuilder(
                    Configuration.GetConnectionString("_3DCRecessWeekChallengeContextDev"));
                _connection = builder.ConnectionString;
            }
           

            services.AddDbContext<_3DCRecessWeekChallengeContext>(options =>
                    options.UseSqlServer(_connection));
            services.AddRazorPages();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
