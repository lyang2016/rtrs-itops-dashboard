using System.Diagnostics.CodeAnalysis;
using System.IO;
using Emma.Config;
using log4net;
using log4net.Appender;
using RTRSCommon;

namespace Dashboard.Common
{
    [ExcludeFromCodeCoverage]
    public static class WebLoggers
    {
        public static readonly ILog ApplicationTrace = LogManager.GetLogger("ApplicationTrace");           
        public static readonly ILog Email = LogManager.GetLogger("Email");

        static WebLoggers()
        {
            RtrsConfigurator.Configure(CurrentApplicationName, GetWebsiteLogFileName);
        }

        private static string GetWebsiteLogFileName(FileAppender appender)
        {
            return Path.Combine(Configuration.Instance.Properties["webservice_log_file_base_directory"], CurrentApplicationName, CurrentApplicationName + "_" + appender.Name + ".log");
        }

        private static string CurrentApplicationName
        {
            get { return Configuration.Instance.Properties["ApplicationName"]; }
        }
    }
}
