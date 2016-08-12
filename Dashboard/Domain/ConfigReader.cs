using System.Configuration;
using System.Linq;
using System.Xml.Linq;
using Dashboard.Models;
using Newtonsoft.Json;

namespace Dashboard.Domain
{
    public class ConfigReader
    {
        private static readonly ConfigReader instance = new ConfigReader();

        // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
        static ConfigReader()
        {
        }

        private ConfigReader()
        {
            
        }

        public static ConfigReader Instance
        {
            get
            {
                return instance;
            }
        }

        public string GetDataCenterConfigs()
        {
            var path = ConfigurationManager.AppSettings["LocalConfigPath"] ?? @"C:\machineconfig\RtrsMetricsConfig.xml";

            var document = XDocument.Load(path);

            var list = document.Descendants("data_center").Select(x => new DataCenterConfig
            {
                SiteId = x.Element("site_id").Value,
                Location = x.Element("site_location").Value,
                WebApiUrl = x.Element("webapi_url").Value,
                LineColor = x.Element("line_color").Value,
                LegendColor = x.Element("legend_color").Value
            }).ToList();

            return JsonConvert.SerializeObject(list);
        }
    }
}