namespace INFINITE.CORE.MVC.Helper
{
    public class CoreHelper
    {
        public required IConfiguration Configuration { get; set; }
        public string? APIURL => Configuration != null ? Configuration["ApplicationConfig:APIUrl"] : string.Empty;
        public string? APIVERSION => Configuration != null ? Configuration["ApplicationConfig:APIVersion"] : string.Empty;
        public string? APPVERSION => Configuration != null ? Configuration["ApplicationConfig:APPVersion"] : string.Empty;
        public string? ISSUER => Configuration != null ? Configuration["ApplicationConfig:Issuer"] : string.Empty;
    }
}
