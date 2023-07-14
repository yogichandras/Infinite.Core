using Microsoft.Extensions.DependencyInjection;
using INFINITE.CORE.Shared.Interface;
using INFINITE.CORE.Shared.Helper;

namespace INFINITE.CORE.Shared
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterShared(this IServiceCollection services)
        {
            services.AddSingleton<IGeneralHelper, GeneralHelper>();
            services.AddSingleton<IWrapperHelper, WrapperHelper>();
            services.AddSingleton<IHttpRequest, HttpRequest>();

            return services;
        }
    }
}
