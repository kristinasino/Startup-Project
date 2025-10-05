using System.Reflection;
using System.Text;
using Autofac;
using Email.Microservice.Core.Configuration;
using FluentValidation;
using Identity.Microservice.Core.Stores;
using Identity.Microservice.Domain.Entities;
using Identity.Microservice.WebHost.Configurations;
using Identity.Microservice.WebHost.Middlewares;
using Infrastructure.Configuration;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using UserModule.Domain.Entities;
using Web.IdentityFactory;

namespace Identity.Microservice.WebHost
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private IWebHostEnvironment Environment { get; }

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, false)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
            Environment = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var userCoreAssembly = Assembly.Load("Identity.Microservice.Core");
            var tokenSection = Configuration.GetSection("TokenConfig");
            var connectionSection = Configuration.GetSection("ConnectionStrings");
            var gatewaySection = Configuration.GetSection("GatewaySettings");
            
            var emailSection = Configuration.GetSection("MailSettings");
// Add services to the container.
            services.Configure<EmailSettings>(emailSection);
            var authenticationSchemeKey = gatewaySection.GetSection("AuthenticationSchemeKey").Value;
            services.Configure<ConnectionSettings>(connectionSection);
            services.Configure<TokenSettings>(tokenSection);
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Web", Version = "v1" }); });
            services.AddDbContexts(Configuration, Environment);

            services.AddIdentityCore<User>()
                .AddRoles<UserRole>()
                .AddUserStore<CustomUserStore>()
                .AddRoleStore<CustomRoleStore>()
                .AddUserManager<CustomUserManager>()
                .AddSignInManager<CustomSignInManager>()
                .AddDefaultTokenProviders();
           
            
            services.Configure<DataProtectionTokenProviderOptions>(opt =>
                opt.TokenLifespan = TimeSpan.FromHours(2));
            
            var key = tokenSection.GetSection("key").Value;
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
            var tokenValidationParameters = new TokenValidationParameters  
            {  
                ValidateIssuerSigningKey = true,  
                IssuerSigningKey = signingKey,  
                ValidateIssuer = true,  
                ValidIssuer = tokenSection.GetSection("Iss").Value,  
                ValidateAudience = true,  
                ValidAudience = tokenSection.GetSection("Aud").Value,  
                ValidateLifetime = true,  
                ClockSkew = TimeSpan.Zero,  
                RequireExpirationTime = true
            };  
  
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)  
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, x =>  
                {  
                    x.RequireHttpsMetadata = false;  
                    x.TokenValidationParameters = tokenValidationParameters;  
                });  
          
            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddHttpContextAccessor();
           
            services.AddAutoMapper(userCoreAssembly);

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });
            
            services.AddMediatR(userCoreAssembly);
            services.AddSharedInfrastructure(Configuration);
            //services.AddValidatorsFromAssembly(userCoreAssembly);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Register your own things directly with Autofac here. Don't
            // call builder.Populate(), that happens in AutofacServiceProviderFactory
            // for you.
            builder.RegisterModule(new ServiceRegistrationModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();
            
            loggerFactory.AddSerilog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web v1"));
            }
            else
                app.UseHsts();
            
            // global error handler
            app.UseMiddleware<ErrorHandlerMiddleware>();
            
            app.UseHttpsRedirection();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseRouting();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}