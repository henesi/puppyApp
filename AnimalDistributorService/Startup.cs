using System;
using System.IO;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using AnimalDistributorService.Api.Services;
using AnimalDistributorService.Configuration;
using AnimalDistributorService.Configuration.IoC;
using AnimalDistributorService.DataAccess.EntityFramework;
using AnimalDistributorService.DataAccess.File;
using AnimalDistributorService.DataAccess.Seed;
using AnimalDistributorService.MessageBroker.Producers;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Contract.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace AnimalDistributorService
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
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var configurationOptions = Configuration.Get<ConfigurationOptions>();

            var key = Encoding.ASCII.GetBytes(configurationOptions.SECRET);

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

            services.AddDbContext<AnimalDBContext>(options => options.UseNpgsql(configurationOptions.CONNECTION_STRING));
            services.AddScoped<IRepository<Animal>, AnimalRepository>();
            services.AddScoped<IRepository<Media>, MediaRepository>();
            services.AddScoped<IMediaService, MediaService>();
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

            //Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AnimalDistributor Api", Version = "v1" });
            });

            services.AddMediatR();
            services.AddAnimalDemoInitializer();

            var autoFacBuilder = new ContainerBuilder();
            autoFacBuilder.Populate(services);
            autoFacBuilder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AsImplementedInterfaces();
            autoFacBuilder.RegisterModule(new MassTransitModule
            {
                ConfigurationOptions = configurationOptions
            });
            autoFacBuilder.RegisterModule(new FileServerModule
            {
                ConfigurationOptions = configurationOptions
            });
            autoFacBuilder.RegisterType<AnimalCreationProducer>();
            autoFacBuilder.RegisterType<AnimalMediaProducer>();
            autoFacBuilder.RegisterType<AnimalProfileProducer>();

            ApplicationContainer = autoFacBuilder.Build();

            return new AutofacServiceProvider(ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime applicationLifetime)
        {
            app.UseRouting();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseSwagger();
            app.UseAuthorization();
            app.UseInitializer();
            app.UseHttpsRedirection();
            app.UseEndpoints(endpoints => endpoints.MapControllers());

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AnimalDistributor Api V1");
            });

            app.UseFileServer(
                new FileServerOptions()
                {
                    FileProvider = new PhysicalFileProvider(Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Media")).FullName)
                }
            );

            applicationLifetime.ApplicationStopped.Register(() => ApplicationContainer.Dispose());
        }
    }
}
