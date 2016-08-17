using System;
using System.Diagnostics.CodeAnalysis;
using System.Transactions;
using Emma.Config;


namespace WorkflowMetricsStager.Domain
{
    public class StagerRunner
    {
        private readonly IConfiguration _config;
        private readonly IStagerDao _dao;

        [ExcludeFromCodeCoverage]
        public StagerRunner()
            : this(Configuration.Instance, new StagerDao())
        {
        }

        public StagerRunner(IConfiguration config, IStagerDao dao)
        {
            _config = config;
            _dao = dao;
        }

        public void Run()
        {
            var siteId = _config.Properties["site_id"];
            var to = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            var stagerSleepInterval = int.Parse(_config.Properties["workflow_stager_sleep_interval_millisecs", true]);

            // Compare stager last interval from now against last run time to determine the from time to poll workflow database
            var from = to.AddMilliseconds(-1.0 * stagerSleepInterval);
            var lastRunTime = _dao.GetLastRunTime();

            if (lastRunTime < from)
            {
                from = lastRunTime;
            }

            using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions(),
                    EnterpriseServicesInteropOption.Full))
            {
                var metricsList = _dao.GetSystemMetricsDataFromWorkflow(siteId, from, to);
                _dao.InsertSystemMetricsData(metricsList);
                scope.Complete();
            }
        }
    }
}
