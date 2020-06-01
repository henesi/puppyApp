using System;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using AnimalDistributorService.Api.Services;
using ComputerVisionService.Configuration;
using ComputerVisionService.Configuration.IoC;
using ComputerVisionService.MessageBroker.Consumers;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using ComputerVisionService.MessageBroker.Producers;

namespace ComputerVisionService
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IContainer ApplicationContainer { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var configurationOptions = Configuration.Get<ConfigurationOptions>();

            var key = Encoding.ASCII.GetBytes(configurationOptions.SECRET);

            services
                .AddControllers()
                .AddControllersAsServices();

            services.Configure<ConfigurationOptions>(options => Configuration.Bind(options));


            services.AddCors(opt => opt.AddPolicy("CorsPolicy",
                builder =>
                {
                    builder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .WithOrigins(configurationOptions.ALLOWED_AUTH_ORIGINS);
                }));

            services.AddMvc()
                .AddNewtonsoftJson()
                .AddControllersAsServices()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("IsSuperUser", policy => policy.RequireClaim(ClaimTypes.Role, "SUPERUSER"));
                options.AddPolicy("IsAdmin", policy => policy.RequireClaim(ClaimTypes.Role, "ADMIN"));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ComputerVision Api", Version = "v1" });
            });

            services.AddMediatR();
        }

        public void ConfigureContainer(ContainerBuilder autoFacBuilder)
        {
            var configurationOptions = Configuration.Get<ConfigurationOptions>();

            autoFacBuilder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AsImplementedInterfaces();
            autoFacBuilder.RegisterModule(new MassTransitModule
            {
                ConfigurationOptions = configurationOptions
            });

            autoFacBuilder.RegisterType<AnimalMediaComputedProducer>();
            autoFacBuilder.RegisterType<AnimalProfileComputedProducer>();

            autoFacBuilder.RegisterModule(new FileServerModule
            {
                ConfigurationOptions = configurationOptions
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseSwagger();
            app.UseAuthorization();
            app.UseHttpsRedirection();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AnimalDistributor Api V1");

            });

            applicationLifetime.ApplicationStopped.Register(() => ApplicationContainer.Dispose());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
