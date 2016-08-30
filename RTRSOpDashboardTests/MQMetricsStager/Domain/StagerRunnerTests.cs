using System;
using Emma.Config;
using MQMetricsStager.Domain;
using NUnit.Framework;
using Rhino.Mocks;
using RTRSCommon;
using RTRSOpDashboard.DataModel;

namespace RTRSOpDashboardTests.MQMetricsStager.Domain
{
    [TestFixture]
    public class StagerRunnerTests
    {
        private IStagerDao _stagerDao;
        private IConfiguration _config;
        private IQueueReader _inboundReader;
        private IQueueReader _outboundReader;

        [SetUp]
        public void Setup()
        {
            _stagerDao = MockRepository.GenerateMock<IStagerDao>();
            //_config = MockRepository.GenerateMock<IConfiguration>();
            _config = Configuration.Instance;
            _inboundReader = MockRepository.GenerateMock<IQueueReader>();
            _outboundReader = MockRepository.GenerateMock<IQueueReader>();
        }

        [Test]
        public void GetQDepth_InsertQDepth_Was_Called()
        {
            // Arrange
            _inboundReader.Expect(x => x.GetCurrentDepth()).Return(100);
            _stagerDao.Expect(x => x.InsertMqCurrentDepth(Arg<MetricsModel>.Is.Anything));
            _outboundReader.Expect(x => x.GetCurrentDepth()).Return(100);

            //_stagerDao.Expect(x => x.InsertMqCurrentDepth(Arg<MetricsModel>.Is.Anything));


            // Act
            var runner = new StagerRunner(_config, _stagerDao, _inboundReader, _outboundReader);
            runner.Run();

            // Assert
            _inboundReader.VerifyAllExpectations();
            _outboundReader.VerifyAllExpectations();

            _stagerDao.VerifyAllExpectations();
        }
    }
}
