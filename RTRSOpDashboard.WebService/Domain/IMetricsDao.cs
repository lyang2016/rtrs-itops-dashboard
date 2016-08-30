using System;
using System.Collections.Generic;
using RTRSOpDashboard.DataModel;

namespace RTRSOpDashboard.WebService.Domain
{
    public interface IMetricsDao
    {
        List<MetricsModel> GetMetricsDataFromLastInterval(DateTime from, DateTime to);
    }
}