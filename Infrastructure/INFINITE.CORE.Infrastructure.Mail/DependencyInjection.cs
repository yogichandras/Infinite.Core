using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using INFINITE.CORE.Infrastructure.Mail.Interface;
using INFINITE.CORE.Infrastructure.Mail.Service;
using INFINITE.CORE.Infrastructure.Mail.Object;

namespace INFINITE.CORE.Infrastructure.Mail
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterMail(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailConfig>(options => configuration.Bind(nameof(MailConfig), options));
            services.AddTransient<IEmailService, EmailService>();
            
            return services;
        }
    }
}
