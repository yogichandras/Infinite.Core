namespace INFINITE.CORE.MVC.Helper
{
    public class CoreHelper
    {
        public IConfiguration Configuration { get; set; }
        public string APIURL => Configuration["ApplicationConfig:APIUrl"];
        public string APIVERSION => Configuration["ApplicationConfig:APIVersion"];
        public string APPVERSION => Configuration["ApplicationConfig:APPVersion"];
    }
}
