using System;
using Emma.Config;
using NUnit.Framework;
using WorkflowMetricsStager.Domain;

namespace RTRSOpDashboardTests.WorkflowMetricsStager.Domain
{
    [TestFixture]
    public class StagerDaoTest2
    {
        private string _connectionString;
        private readonly string _messageId = new string('3', 32);


        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _connectionString = string.Format("Data Source={0};User Id={1};Password={2}",
                Configuration.Instance.Properties["DB_SOURCE_WRITE"], Configuration.Instance.Properties["LOG_USER_ID"],
                Configuration.Instance.Properties["LOG_USER_PASSWORD"]);
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
            TestHelper.RunQuery("delete from rtrsapp.LOG_WKFW_WINDOW where MESSAGE_ID in ('{0}')", _messageId);
        }

        [Test]
        public void GetSystemMetricsDataFromWorkflow()
        {
            // Arrange
            InsertWorkflowLog(_messageId, DateTime.Parse("1999-1-1 10:00:00"));

            // Act
            var stagerDao = new StagerDao();
            var list = stagerDao.GetSystemMetricsDataFromWorkflow(_messageId, DateTime.Parse("1999-1-1 9:00:00"), DateTime.Parse("1999-1-1 11:00:00"));

            // Assert
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(1, list[0].MessageCount);
        }


        private static void InsertWorkflowLog(string messageId, DateTime completionSecond)
        {
            TestHelper.RunQuery(
                "INSERT INTO rtrsapp.LOG_WKFW_WINDOW (MESSAGE_ID, WORKFLOW_ID, PATH_SEGMENT_ID, PREV_STATUS_ID, PREV_STATUS_DT, NEW_STATUS_ID, NEW_STATUS_DT, LOG_DT) " +
                "values ('{0}', 1, 0, 2, to_date('{1}', 'DD-MON-YYYY HH:MI:SS AM'), 1, to_date('{2}', 'DD-MON-YYYY HH:MI:SS AM'), to_date('{3}', 'DD-MON-YYYY HH:MI:SS AM'))",
                messageId, TestHelper.FormatOracleDateTime(completionSecond), TestHelper.FormatOracleDateTime(completionSecond), TestHelper.FormatOracleDateTime(completionSecond));
        }

    }
}
