using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace OnlineBankamatik
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(_ =>
            {
                _.DefaultScheme = "OnlineBankamatikCookie";
                _.DefaultChallengeScheme = "oidc";
            })
           .AddCookie("OnlineBankamatikCookie", options => options.AccessDeniedPath = "/home/accessdenied")
           .AddOpenIdConnect("oidc", _ =>
           {
               _.SignInScheme = "OnlineBankamatikCookie";
               _.Authority = "https://localhost:5000";
               _.ClientId = "OnlineBankamatik";
               _.ClientSecret = "onlinebankamatik";
               _.ResponseType = "code id_token";
               _.GetClaimsFromUserInfoEndpoint = true;

               _.SaveTokens = true;
               _.Scope.Add("offline_access");

               _.Scope.Add("Garanti.Write");
               _.Scope.Add("Garanti.Read");
               _.Scope.Add("PositionAndAuthority");
               _.ClaimActions.MapUniqueJsonKey("position", "position");
               _.ClaimActions.MapUniqueJsonKey("authority", "authority");

               _.Scope.Add("UserInfo");
               _.ClaimActions.MapUniqueJsonKey("name", "name");
               _.ClaimActions.MapUniqueJsonKey("website", "website");

               _.Scope.Add("Roles");
               _.ClaimActions.MapUniqueJsonKey("role", "role");
               _.TokenValidationParameters = new TokenValidationParameters
               {
                   RoleClaimType = "role"
               };

               _.Scope.Add("GobekAdi");
               _.ClaimActions.MapUniqueJsonKey("gobekadi", "gobekadi");

           });
            services.AddHttpClient();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("Default", "{controller=Bankamatik}/{action=Index}");
            });
        }
    }
}
