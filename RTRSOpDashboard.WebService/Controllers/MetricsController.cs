﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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
        }

    }
}
