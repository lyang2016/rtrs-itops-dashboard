using System;
using System.Collections.Generic;
using System.Data;
using Emma.Config;
using RTRSOpDashboard.DataModel;

namespace RTRSOpDashboard.WebService.Domain
{
    public class MetricsDao : IMetricsDao
    {
        public List<MetricsModel> GetMetricsDataFromLastInterval(DateTime from, DateTime to)
        {
            var metricsList = new List<MetricsModel>();

            using (var conn = Database.Instance.ReadConnection)
            {
                conn.Open();
                using (var cmdx = conn.CreateCommandEx())
                {
                    var cmd = cmdx.Cmd;
                    cmd.CommandText = "rtrsmetrics.pkg_metrics.sp_get_metrics_by_time";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.BindByName(true);
                    cmd.AddParameter("P_DATE_FROM", ODT.Date, from);
                    cmd.AddParameter("P_DATE_TO", ODT.Date, to);
                    cmd.AddOutputParameter("P_DATA", ODT.Cursor);


                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var metricsRecord = new MetricsModel
                            {
                                SiteId = reader["SITE_ID"] as string,
                                CompletionSecond = to,
                                WorkflowId = reader["WORKFLOW_ID"] as decimal?,
                                MessageCount = reader["msg_count_completed"] as decimal?
                            };

                            metricsList.Add(metricsRecord);
                        }
                    }
                }
            }

            return metricsList;
        }

    }
}