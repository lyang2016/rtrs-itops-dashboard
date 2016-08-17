using System;
using System.Collections.Generic;
using RTRSOpDashboard.DataModel;

namespace WorkflowMetricsStager.Domain
{
    public interface IStagerDao
    {
        List<MetricsModel> GetSystemMetricsDataFromWorkflow(string siteId, DateTime from, DateTime to);
        void InsertSystemMetricsData(List<MetricsModel> metricsList);
        DateTime GetLastRunTime();
    }
}