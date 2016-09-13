using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.Http;
using System.Web.Mvc;
using Dashboard.Common;
using WebApiContrib.Formatting.Jsonp;

namespace RTRSOpDashboard.WebService
{
    [ExcludeFromCodeCoverage]
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.AddJsonpFormatter();
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            if (exception != null)
            {
                WebLoggers.ApplicationTrace.ErrorFormat("Exception from RTRSDashboardWebService - {0}", exception.Message);                
            }

            // Clear the error
            Server.ClearError();
        }
    }
}
