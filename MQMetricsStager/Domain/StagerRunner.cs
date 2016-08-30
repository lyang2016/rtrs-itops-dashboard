using System;
using System.Diagnostics.CodeAnalysis;
using System.Transactions;
using Emma.Config;
using RTRSCommon;
using RTRSOpDashboard.DataModel;

namespace MQMetricsStager.Domain
{
    public class StagerRunner
    {
        private readonly IConfiguration _config;
        private readonly IStagerDao _dao;
        private readonly IQueueReader _inboundReader;
        private readonly IQueueReader _outboundReader;

        [ExcludeFromCodeCoverage]
        public StagerRunner()
            : this(Configuration.Instance, new StagerDao(), new QueueReader(new MqConfig().GetSettings("dq_")), new QueueReader(new MqConfig().GetSettings("responder_")))
        {
        }

        public StagerRunner(IConfiguration config, IStagerDao dao, IQueueReader inboundReader, IQueueReader outboundReader)
        {
            _config = config;
            _dao = dao;
            _inboundReader = inboundReader;
            _outboundReader = outboundReader;
        }

        public void Run()
        {
            var siteId = _config.Properties["site_id"];
            var to = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            var stagerSleepInterval = int.Parse(_config.Properties["mq_stager_sleep_interval_millisecs", true]);

            // Compare stager last interval from now against last run time to determine the from time to poll workflow database
            var from = to.AddMilliseconds(-1.0 * stagerSleepInterval);

            var qDepth = 0;
            var record = new MetricsModel
            {
                SiteId = siteId,
                CompletionSecond = to,
                MessageCount = qDepth,
                WorkflowId = 0
            };

            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions(),
                    EnterpriseServicesInteropOption.Full))
                {
                    // Inbound QDepth
                    qDepth = _inboundReader.GetCurrentDepth();
                    record.MessageCount = qDepth;
                    record.WorkflowId = 101;
                    _dao.InsertMqCurrentDepth(record);

                    // Outbound QDepth
                    qDepth = _outboundReader.GetCurrentDepth();
                    record.MessageCount = qDepth;
                    record.WorkflowId = 102;
                    _dao.InsertMqCurrentDepth(record);

                    // Commit the whole transaction
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                Loggers.ApplicationTrace.ErrorFormat("Mq Metrics stager exception: {0}", ex.Message);
                throw;
            }
            finally
            {
                _inboundReader.Reset();
                _outboundReader.Reset();
            }
        }

    }
}
