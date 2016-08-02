using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Configuration = Emma.Config.Configuration;

namespace ChartTest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Chart()
        {
            ViewBag.Message = "Highchart Test";

            return View();
        }


        [HttpGet]
        public async Task<JsonResult> GetMetricsData(DateTime from, DateTime to)
        {
            var paramDict = new Dictionary<string, string>
            {
                {"from", from.ToString("yyyy-MM-dd HH:mm:ss")},
                {"to", to.ToString("yyyy-MM-dd HH:mm:ss")}
            };

            HttpResponseMessage response;
            //var baseUrl = ConfigurationManager.AppSettings["SystemMetricsApiBaseUrl"];
            var baseUrl = Configuration.Instance.Properties["webapi_base_url", true];

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var apiUri = baseUrl + "Metrics/GetSystemMetricsData?from=" + from.ToString("yyyy-MM-dd HH:mm:ss") + "&to=" + to.ToString("yyyy-MM-dd HH:mm:ss");

                response = await client.GetAsync(apiUri);
            }

            return Json(response.Content.ReadAsStringAsync(), JsonRequestBehavior.AllowGet);
        }


    }
}