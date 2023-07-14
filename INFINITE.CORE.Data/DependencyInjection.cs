using INFINITE.CORE.Data.Base;
using INFINITE.CORE.Data.Base.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace INFINITE.CORE.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterData(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDBContext>(x => x.UseSqlServer(
                configuration.GetConnectionString("MainConnection"), (option) =>
                {
                    option.CommandTimeout(3600);
                    option.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                }));
            services.AddTransient(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            return services;
        }
    }
}
