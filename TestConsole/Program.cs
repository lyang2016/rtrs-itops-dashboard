using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTRSCommon;
using RTRSOpDashboard.WebService.Domain;
using WorkflowMetricsStager;
using IBM.WMQ;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //var testDao = new TestDao();
            //testDao.GetTestData();

            //var metricsDao = new MetricsDao();
            //metricsDao.GetMetricsDataFromLastInterval(DateTime.Parse("2016-06-29 10:00:10"), DateTime.Parse("2016-06-29 10:00:15"));

            //var mockMetricsDao = new MockMetricsDao();
            //mockMetricsDao.GetMetricsDataFromLastInterval();

            var mqSetting = new MqConfig().GetSettings("dq_");

            using (var mqConnection = new MqConnection(mqSetting))
            {
                mqConnection.OpenQueue();

                var currentLength = mqConnection.GetCurrentDepth();
                Console.WriteLine("Current Length = {0}", currentLength);

                List<string> messagList = new List<string> {"Hello MQ"};

                mqConnection.PutMessages(messagList);

                currentLength = mqConnection.GetCurrentDepth();
                Console.WriteLine("Current Length = {0}", currentLength);

            }


            Console.ReadLine();
        }
    }
}
