using System;
using System.Collections.Generic;
using RTRSOpDashboard.DataModel;

namespace WorkflowMetricsStager.Domain
{
    public interface IStagerDao
    {
        List<MetricsModel> GetSystemMetricsDataFromWorkflow(string siteId, DateTime from, DateTime to);
        void InsertSystemMetricsData(List<MetricsModel> metricsList);
<<<<<<< HEAD
        DateTime GetLastRunTime();
=======
>>>>>>> e0e1ff74d1444cc92b0949760451e6c7a2b14f0a
    }
}