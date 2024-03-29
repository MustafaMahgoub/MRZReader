﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using MRZReader.Core;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MRZReader.Dal;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using MRZReader.DataExtractor;
using MRZReader.Handlers;

namespace MRZReader.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<MrzReaderDbContext>();

            // Custome password if needed
            //services.Configure<IdentityOptions>(options =>
            //{
            //    options.Password.RequiredLength = 10;
            //    options.Password.RequiredUniqueChars = 3;
            //    options.Password.RequireNonAlphanumeric = false;
            //    options.Password.RequireDigit = false;
            //});


            services.AddHttpClient("TruliooClient", client =>
            {
                client.BaseAddress = new Uri("https://gateway.trulioo.com/trial/");
                client.Timeout = TimeSpan.FromSeconds(30);
                client.DefaultRequestHeaders.Add("x-trulioo-api-key", "42f5e1dabd01c87d68dad5790b0ef3a6");
            });

            //services.AddHttpClient("MRZClient", client =>
            //{
            //    client.BaseAddress = new Uri("https://localhost:44381/");
            //    client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //});

            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IDataExtractor, XmlDataExtractor>();
            services.AddMediatR(typeof(MRZReaderHandler).GetTypeInfo().Assembly);

            services.AddOptions();
            services.Configure<CloudOcrSettings>(Configuration.GetSection("CloudOcrSettings"));
            services.Configure<TruliooSettings>(Configuration.GetSection("TruliooSettings"));
            services.Configure<DocumentStorageSettings>(Configuration.GetSection("DocumentStorageSettings"));
            services.Configure<CacheSettings>(Configuration.GetSection("Cache"));
            services.AddDbContextPool<MrzReaderDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("MRZReaderDBConnection")));

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc(
                // To apply authorization globally
                //options =>
                //{
                // var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                //  options.Filters.Add(new AuthorizeFilter(policy));
                //}
                ).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
