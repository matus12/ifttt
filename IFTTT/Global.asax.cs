using System.Web.Http;

namespace IFTTT
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configure(ApiFormatConfig.Register);
            GlobalConfiguration.Configure(DependenciesConfig.Register);
        }
    }
}
