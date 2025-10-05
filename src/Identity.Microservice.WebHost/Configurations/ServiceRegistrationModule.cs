using System.Reflection;
using Autofac;
using Identity.Microservice.WebHost.Middlewares;
using Infrastructure.DAL.Factory;
using Infrastructure.UnitOfWork;
using MediatR;
using Shared.Common.Extensions;
using Shared.Infrastructure.UnitOfWork;
using UserModule.Infrastructure.DAL.Factory;
using Module = Autofac.Module;

namespace Identity.Microservice.WebHost.Configurations;

public class ServiceRegistrationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<UnitOfWork>().As<IUnitOfWork>()
            .InstancePerLifetimeScope();
        builder.RegisterType<ContextFactory>().As<IContextFactory>()
            .InstancePerLifetimeScope();
        builder.RegisterType<HttpContextAccessor>()
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
        builder.RegisterType<ErrorHandlerMiddleware>()
            .InstancePerDependency();
        builder.RegisterGeneric(typeof(ValidationBehavior<,>)).As(typeof(IPipelineBehavior<,>))
            .InstancePerDependency();
        

        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(x => x.FullName!.Contains("Identity.Microservice.Core")).ToList();

        foreach (var assembly in assemblies)
        {
            builder.RegisterAssemblyTypes(Assembly.Load(assembly.GetName()))
                .Where(t => t.Name.EndsWith("Service") || t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
        // Scan an assembly for components
    }
}