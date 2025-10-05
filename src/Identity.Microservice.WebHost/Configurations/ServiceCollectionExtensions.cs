using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using UserModule.Infrastructure.DAL.EfContext;

namespace Identity.Microservice.WebHost.Configurations
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllers()
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    opt.JsonSerializerOptions.WriteIndented = true;
                    opt.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                });
            return services;
        }
        
        public static void AddDbContexts(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            var contextConnectionString = configuration.GetConnectionString("Db");
            services.AddDbContextPool<DataContext>(x => x.UseSqlServer(contextConnectionString, o =>
                {
                    o.EnableRetryOnFailure(3);
                })
                .EnableSensitiveDataLogging(environment.IsDevelopment())
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
        }
    }
}