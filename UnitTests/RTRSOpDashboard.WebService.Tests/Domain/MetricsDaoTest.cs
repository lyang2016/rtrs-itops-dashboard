using System;
using Emma.Config;
using NUnit.Framework;
using RTRSOpDashboard.WebService.Domain;

namespace RTRSOpDashboard.WebService.Tests.Domain
{
    [TestFixture]
    public class MetricsDaoTest
    {
        private string _connectionString;
        private const string siteId = "LY";

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _connectionString = string.Format("Data Source={0};User Id={1};Password={2}",
                Configuration.Instance.Properties["DB_SOURCE_WRITE"], Configuration.Instance.Properties["DB_USER_ID"],
                Configuration.Instance.Properties["DB_USER_PASSWORD"]);
            TestHelper.ConnectionString = _connectionString;

            CleanUp();
        }

        [TearDown]
        public void TearDown()
        {
            CleanUp();
        }

        private void CleanUp()
        {
            TestHelper.RunQuery("delete from rtrsmetrics.site_module_thruput_by_sec where site_id in ('{0}')", siteId);
        }

        [Test]
        public void GetMetricsDataFromLastInterval()
        {
            InsertMetricsData(siteId, DateTime.Parse("1999-1-1 10:00:00"), 100, 100);
            var metricsDao = new MetricsDao();
            var result = metricsDao.GetMetricsDataFromLastInterval(DateTime.Parse("1999-1-1 9:00:00"), DateTime.Parse("1999-1-1 11:00:00"));
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(100, result[0].MessageCount);
        }

        private static void InsertMetricsData(string siteId, DateTime completionSecond, short workflowId, decimal messageCount)
        {
            TestHelper.RunQuery("insert into rtrsmetrics.site_module_thruput_by_sec(site_id, completion_second, workflow_id, message_count_completed) values ('{0}', to_date('{1}', 'DD-MON-YYYY HH:MI:SS AM'), {2}, {3})",
                siteId, TestHelper.FormatOracleDateTime(completionSecond), workflowId, messageCount);
        }


    }
}
