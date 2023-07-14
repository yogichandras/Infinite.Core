using INFINITE.CORE.Core.Helper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace INFINITE.CORE.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.Configure<ApplicationConfig>(options => configuration.Bind(nameof(ApplicationConfig), options));
           
            var type = typeof(DependencyInjection);
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(d => d.FullName.StartsWith(type.Namespace!)).SelectMany(d => d.DefinedTypes);


            var interfaces = assemblies.Where(d => d.IsInterface).ToList();
            foreach (var @interface in interfaces)
            {
                var @class = assemblies.Where(x => @interface.IsAssignableFrom(x) && !x.IsInterface)
                                       .OrderByDescending(x => x.Name).FirstOrDefault();
                if (@class != null)
                {
                    services.AddTransient(@interface, @class);
                }
            }
            return services;
        }
    }
}
