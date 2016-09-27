using System;
<<<<<<< HEAD
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Dashboard.Common;

namespace Dashboard
{
    [ExcludeFromCodeCoverage]
=======
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Dashboard
{
>>>>>>> e0e1ff74d1444cc92b0949760451e6c7a2b14f0a
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
<<<<<<< HEAD

        protected void Application_Error(Object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            if (exception != null)
            {
                WebLoggers.ApplicationTrace.ErrorFormat("Exception from RTRSITOpsDashboard - {0}", exception.Message);
            }

            // Clear the error
            Server.ClearError();
        }
=======
>>>>>>> e0e1ff74d1444cc92b0949760451e6c7a2b14f0a
    }
}
