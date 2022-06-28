using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrebovanjeBackendERP.Entities;
using TrebovanjeBackendERP.Repositories;

namespace TrebovanjeBackendERP
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
            // If using Kestrel:
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            // If using IIS:
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TrebovanjeBackendERP", Version = "v1" });
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admins", policy =>
                                  policy.RequireClaim("IsAdmin", "1"));
            });

            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IDistributerRepository, DistributerRepository>();
            services.AddScoped<IKorisnikRepository, KorisnikRepository>();
            services.AddScoped<ILokacijaRepository, LokacijaRepository>();
            services.AddScoped<IPorudzbinaRepository, PorudzbinaRepository>();
            services.AddScoped<IProizvodRepository, ProizvodRepository>();
            services.AddScoped<IStavkaPorudzbineRepository, StavkaPorudzbineRepository>();
            services.AddScoped<ILokacijaRepository, LokacijaRepository>();
            services.AddScoped<IProizvodRepository, ProizvodRepository>();
            services.AddScoped<IKategorijaProizvodaRepository, KategorijaProizvodaRepository>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddMvc();

            services.AddDbContextPool<TrebovanjeDatabaseContext>(options => options.UseSqlServer(Configuration.GetConnectionString("TrebovanjeDB")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            StripeConfiguration.ApiKey = "sk_test_51LEHoBFYog7W2g6eg9hDQ2A4TRB19jX5fNOubYYpfG5fi8kP6OwfIgp0SbSggROc5afeWiY344VwbyMg2zyFyoTH00UxmcHz0p";

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TrebovanjeBackendERP v1"));
            }

            app.UseHttpsRedirection();

            app.UseCors(x => x
              .AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());

            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
