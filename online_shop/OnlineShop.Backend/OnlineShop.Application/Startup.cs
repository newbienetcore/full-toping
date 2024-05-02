using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineShop.Application.Helpers;
using OnlineShop.Application.Middleware;
using OnlineShop.Core.Interfaces;
using OnlineShop.Core.Models;
using OnlineShop.Infrastructure;
using OnlineShop.Infrastructure.Services;
using System.Text;

namespace OnlineShop.Application
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
            services.AddSingleton<IMailService, MailService>();
            services.AddOptions();
            services.Configure<MailSettings>(Configuration.GetSection(nameof(MailSettings)));
            services.AddSession();
            services.AddHttpContextAccessor(); // Register IHttpContextAccessor
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            MappingConfig.AutoMapperConfig(services);
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                    builder.SetIsOriginAllowed(_ => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
            services.AddDbContext<DataContext>(
                options =>
                {
                    options.UseMySql(
                        Configuration.GetConnectionString("Default"),
                        ServerVersion.Parse("8.0.23-mysql"),
                        mySqlOptions =>
                        {
                            mySqlOptions.MigrationsAssembly("OnlineShop.Application");
                        });
                }, ServiceLifetime.Singleton);

            services.AddSingleton<IDataContext, DataContext>();
            services.AddRazorPages();
            services.AddControllersWithViews();
            services.AddMvc();
            string secretKey = Configuration["Configuration:SecretKey"];
            byte[] key = Encoding.ASCII.GetBytes(secretKey);
            services.AddAuthentication(
                CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(option =>
                {
                    option.LoginPath = "/Auth/Login";
                });
            //services.AddAuthentication(x =>
            //{
            //    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(x =>
            //{
            //    x.RequireHttpsMetadata = false;
            //    x.SaveToken = true;
            //    x.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(key),
            //        ValidateIssuer = false,
            //        ValidateAudience = false
            //    };
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseCors();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseMiddleware<JwtMiddleware>();
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "Default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
