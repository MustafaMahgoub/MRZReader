using System;
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
using Microsoft.AspNetCore.Identity;

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


            services.AddHttpClient();
            services.AddHttpClient("MRZClient", client =>
            {
                client.BaseAddress = new Uri("https://localhost:44381/");
                client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddDbContextPool<MrzReaderDbContext>(
                options=>options.
                    UseSqlServer(Configuration.GetConnectionString("MRZReaderDBConnection")));
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddMediatR(typeof(MRZReaderHandler).GetTypeInfo().Assembly);
            services.AddOptions();
            services.Configure<CloudOcrSettings>(Configuration.GetSection("CloudOcrSettings"));
            services.Configure<DocumentStorageSettings>(Configuration.GetSection("DocumentStorageSettings"));
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
