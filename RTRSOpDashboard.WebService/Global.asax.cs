using System;
<<<<<<< HEAD
using System.Diagnostics.CodeAnalysis;
using System.Web.Http;
using System.Web.Mvc;
using Dashboard.Common;
=======
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
>>>>>>> e0e1ff74d1444cc92b0949760451e6c7a2b14f0a
using WebApiContrib.Formatting.Jsonp;

namespace RTRSOpDashboard.WebService
{
<<<<<<< HEAD
    [ExcludeFromCodeCoverage]
=======
>>>>>>> e0e1ff74d1444cc92b0949760451e6c7a2b14f0a
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
<<<<<<< HEAD

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
=======
>>>>>>> e0e1ff74d1444cc92b0949760451e6c7a2b14f0a
    }
}
