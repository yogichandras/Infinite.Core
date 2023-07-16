using Autofac;
using INFINITE.CORE.Data.Model;
using INFINITE.CORE.MVC.Authorization;
using INFINITE.CORE.MVC.Base;
using INFINITE.CORE.MVC.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace INFINITE.CORE.MVC
{
    public static class Container
    {
        public static void Setup(ContainerBuilder container)
        {
            // Determine the environment
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // Create an instance of IConfiguration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Default configuration file
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true) // Environment-specific configuration file
                .Build();


            container.RegisterInstance(configuration).As<IConfiguration>();
            container.RegisterType<AuthHelper>().PropertiesAutowired();
            container.RegisterType<CoreHelper>().PropertiesAutowired();

            // Get the assembly where the controllers are located
            Assembly assembly = Assembly.GetExecutingAssembly();

            // Get all public non-abstract classes that derive from Controller
            var controllerTypes = assembly.GetTypes()
                .Where(type => (typeof(CoreController).IsAssignableFrom(type) || typeof(CoreViewComponent).IsAssignableFrom(type)) && !type.IsAbstract && type.IsPublic);

            // Register each controller type
            foreach (var controllerType in controllerTypes)
            {
                container.RegisterType(controllerType).PropertiesAutowired();
            }

            
        }
    }
}
