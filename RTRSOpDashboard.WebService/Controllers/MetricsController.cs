using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
<<<<<<< HEAD
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dashboard.Common;
=======
using System.Net;
using System.Net.Http;
using System.Web.Http;
>>>>>>> e0e1ff74d1444cc92b0949760451e6c7a2b14f0a
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
<<<<<<< HEAD
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
=======
        public HttpResponseMessage GetSystemMetricsData(DateTime from, DateTime to)
        {
            List<MetricsModel> metricsList;
            try
            {
                metricsList = _metricsDao.GetMetricsDataFromLastInterval(from, to);
            }
            catch (Exception ex)
            {
                Loggers.ApplicationTrace.ErrorFormat("Exception from GetMetricsData - {0}", ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            return Request.CreateResponse(HttpStatusCode.OK, metricsList);
>>>>>>> e0e1ff74d1444cc92b0949760451e6c7a2b14f0a
        }

    }
}
