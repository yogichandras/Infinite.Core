
namespace INFINITE.CORE.Core.Helper
{
    public class ApplicationConfig
    {
        public int MaximumLoginRetry { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
        public int TokenExpired { get; set; }
        public string MediaPath { get; set; }
    }
}
