using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emma.Config;


namespace WorkflowMetricsStager.Domain
{
    public class StagerRunner
    {
        private readonly IConfiguration _config;
        private readonly IStagerDao _dao;

        [ExcludeFromCodeCoverage]
        public StagerRunner():this(Configuration.Instance, new StagerDao())
        {
        }

        public StagerRunner(IConfiguration config, IStagerDao dao)
        {
            _config = config;
            _dao = dao;
        }

        public void Run()
        {
            var metricsList = _dao.GetSystemMetricsData();
            _dao.InsertSystemMetricsData(metricsList);
        }
    }
}
