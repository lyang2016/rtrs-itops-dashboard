using System;
using System.Collections.Generic;
using System.Data;
using Emma.Config;
using NUnit.Framework;
using RTRSOpDashboard.DataModel;
using RTRSOpDashboard.WebService.Domain;
using WorkflowMetricsStager.Domain;

namespace RTRSOpDashboardTests.WorkflowMetricsStager.Domain
{
    [TestFixture]
    public class StagerDaoTest
    {
        private string _connectionString;
        private const string SiteId = "LY";

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
            TestHelper.RunQuery("delete from rtrsmetrics.site_module_thruput_by_sec where site_id in ('{0}')", SiteId);
        }

        [Test]
        public void InsertSystemMetricsData()
        {
            var metricsData = new MetricsModel
            {
                SiteId = SiteId,
                WorkflowId = 1,
                CompletionSecond = DateTime.Parse("1999-1-1 9:00:00"),
                MessageCount = 100
            };

            var list = new List<MetricsModel>();
            list.Add(metricsData);

            var stagerDao = new StagerDao();
            stagerDao.InsertSystemMetricsData(list);

            var dt = GetSystemMetricsData();
            Assert.AreEqual(1, dt.Rows.Count);
            Assert.AreEqual(100, dt.Rows[0]["MESSAGE_COUNT_COMPLETED"]);
        }

        [Test]
        public void GetLastRunTime()
        {
            // Arrange
            InsertTestSystemMetricsData(DateTime.Parse("2999-1-1 10:00:00"));

            // Act
            var stagerDao = new StagerDao();
            var lastRunTime = stagerDao.GetLastRunTime();

            // Assert
            Assert.AreEqual(DateTime.Parse("2999-1-1 10:00:00"), lastRunTime);
        }

        private static DataTable GetSystemMetricsData()
        {
           return TestHelper.GetData("select * from rtrsmetrics.site_module_thruput_by_sec where site_id = '{0}'", SiteId);
        }

        private static void InsertTestSystemMetricsData(DateTime completionSecond)
        {
            TestHelper.RunQuery(
                "INSERT INTO RTRSMETRICS.SITE_MODULE_THRUPUT_BY_SEC(SITE_ID, COMPLETION_SECOND, WORKFLOW_ID, MESSAGE_COUNT_COMPLETED) " +
                "values ('{0}', to_date('{1}', 'DD-MON-YYYY HH:MI:SS AM'), 1, 100)", SiteId, TestHelper.FormatOracleDateTime(completionSecond));
        }
    }
}
