using System.Collections.Generic;
using RTRSOpDashboard.DataModel;

namespace WorkflowMetricsStager.Domain
{
    public interface IStagerDao
    {
        List<MetricsModel> GetSystemMetricsData();
        void InsertSystemMetricsData(List<MetricsModel> metricsList);
    }
}