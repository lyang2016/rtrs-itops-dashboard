using System;
<<<<<<< HEAD
using System.Diagnostics.CodeAnalysis;
=======
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
>>>>>>> e0e1ff74d1444cc92b0949760451e6c7a2b14f0a
using System.Transactions;
using Emma.Config;
using RTRSCommon;


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
<<<<<<< HEAD

            // Compare stager last interval from now against last run time to determine the from time to poll workflow database
            var from = to.AddMilliseconds(-1.0 * stagerSleepInterval);
            var lastRunTime = _dao.GetLastRunTime();

            if (lastRunTime < from)
            {
                from = lastRunTime;
            }

            //todo Remove the hard-coded from date when deploying
            //from = new DateTime(2016, 7, 28, 11, 0, 0);
=======
            //todo compare stager last interval from now against last run time to determine the from time to poll workflow database
            var from = to.AddMilliseconds(-1.0 * stagerSleepInterval);
>>>>>>> e0e1ff74d1444cc92b0949760451e6c7a2b14f0a

            using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions(),
                    EnterpriseServicesInteropOption.Full))
            {
                var metricsList = _dao.GetSystemMetricsDataFromWorkflow(siteId, from, to);
<<<<<<< HEAD
                Loggers.ApplicationTrace.DebugFormat("Workflow Metrics Stager retrieves # {0} of workflow records", metricsList.Count);
=======
>>>>>>> e0e1ff74d1444cc92b0949760451e6c7a2b14f0a
                _dao.InsertSystemMetricsData(metricsList);
                scope.Complete();
            }
        }
    }
}
