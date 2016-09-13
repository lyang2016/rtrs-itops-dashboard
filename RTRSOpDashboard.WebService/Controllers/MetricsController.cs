using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dashboard.Common;
using RTRSCommon;
using RTRSOpDashboard.DataModel;
using RTRSOpDashboard.WebService.Domain;

namespace RTRSOpDashboard.WebService.Controllers
{
    public class MetricsController : ApiController
    {
        private readonly IMetricsDao _metricsDao;

        [ExcludeFromCodeCoverage]
        public MetricsController() : this(new MetricsDao())
        {
            
        }

        public MetricsController(IMetricsDao metricsDao)
        {
            _metricsDao = metricsDao;
        }

        [HttpGet]
        public IHttpActionResult GetSystemMetricsData(DateTime from, DateTime to, short workflowId)
        {
            var metricsList = new List<MetricsModel>();
            try
            {
                metricsList = _metricsDao.GetMetricsDataFromLastInterval(from, to).Where(x => x.WorkflowId == workflowId).ToList();
            }
            catch (Exception ex)
            {
                WebLoggers.ApplicationTrace.ErrorFormat("Exception from GetMetricsData - {0}", ex.Message);
                return Content(HttpStatusCode.OK, metricsList);
            }

            return Ok(metricsList);
        }

    }
}
