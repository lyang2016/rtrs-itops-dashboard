using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NUnit.Framework;
using Rhino.Mocks;
using RTRSOpDashboard.DataModel;
using RTRSOpDashboard.WebService.Controllers;
using RTRSOpDashboard.WebService.Domain;

namespace RTRSOpDashboardTests.WebService.Controllers
{
    [TestFixture]
    public class MetricsControllerTests
    {
        private IMetricsDao _metricsDao;
        private MetricsController _controller;

        [SetUp]
        public void Setup()
        {
            _metricsDao = MockRepository.GenerateMock<IMetricsDao>();

            _controller = new MetricsController(_metricsDao)
            {
                // Initialize the request in controller to prevent NULL error
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
        }

        [Test]
        public void GetMetricsDataFromLastInterval_Was_Called()
        {
            // Arrange
            var from = DateTime.Now;
            var to = from.AddSeconds(1);

            // Act
            var response = _controller.GetSystemMetricsData(from, to);

            // Assert
            _metricsDao.AssertWasCalled(x => x.GetMetricsDataFromLastInterval(from, to));
        }

        [Test]
        public void GetSystemMetricsData()
        {
            var from = DateTime.Parse("2000-1-1 10:00:00");
            var to = DateTime.Parse("2000-1-1 11:00:00");
            var record = new MetricsModel
            {
                SiteId = "D4",
                WorkflowId = 1,
                CompletionSecond = new DateTime(2000, 1, 1, 10, 30, 0),
                MessageCount = 100
            };
            var list = new List<MetricsModel>();
            list.Add(record);
            _metricsDao.Stub(dao => dao.GetMetricsDataFromLastInterval(from, to)).Return(list);
            List<MetricsModel> output;

            var response = _controller.GetSystemMetricsData(from, to);

            Assert.IsTrue(response.TryGetContentValue<List<MetricsModel>>(out output));
            Assert.AreEqual(1, output.Count);
            Assert.AreEqual(100, output[0].MessageCount);
        }

        [Test]
        public void GetMetricsDataFromLastInterval_Throw_Exception()
        {
            // Arrange
            var from = DateTime.Now;
            var to = from.AddSeconds(1);
            _metricsDao.Stub(dao => dao.GetMetricsDataFromLastInterval(from, to))
                .IgnoreArguments()
                .Repeat.Any()
                .Throw(new Exception());

            // Act
            var response = _controller.GetSystemMetricsData(from, to);

            // Assert
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}
