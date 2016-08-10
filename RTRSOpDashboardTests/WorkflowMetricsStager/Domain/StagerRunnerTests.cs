using System;
using System.Collections.Generic;
using Emma.Config;
using NUnit.Framework;
using Rhino.Mocks;
using RTRSOpDashboard.DataModel;
using WorkflowMetricsStager.Domain;

namespace RTRSOpDashboardTests.WorkflowMetricsStager.Domain
{
    [TestFixture]
    public class StagerRunnerTests
    {
        private IStagerDao _stagerDao;
        private IConfiguration _config;

        [SetUp]
        public void Setup()
        {
            _stagerDao = MockRepository.GenerateMock<IStagerDao>();
            //_config = MockRepository.GenerateMock<IConfiguration>();
            _config = Configuration.Instance;
        }

        [Test]
        public void GetWorkflowData_InsertMetricsData_Was_Called()
        {
            // Arrange
            var list = new List<MetricsModel>
            {
                new MetricsModel
                {
                    SiteId = "LY",
                    CompletionSecond = new DateTime(1999, 1, 1, 10, 0, 0),
                    MessageCount = 100,
                    WorkflowId = 1
                }
            };

            _stagerDao.Expect(
                x =>
                    x.GetSystemMetricsDataFromWorkflow(Arg<string>.Is.Anything, Arg<DateTime>.Is.Anything,
                        Arg<DateTime>.Is.Anything)).Return(list);

            _stagerDao.Expect(x => x.InsertSystemMetricsData(list));

            // Act
            var runner = new StagerRunner(_config, _stagerDao);
            runner.Run();

            // Assert
            _stagerDao.VerifyAllExpectations();
        }
    }
}
