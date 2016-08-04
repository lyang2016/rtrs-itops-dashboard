using System;
using System.Collections.Generic;
using System.Data;
using Emma.Config;
using RTRSCommon;
using RTRSOpDashboard.DataModel;

namespace WorkflowMetricsStager.Domain
{
    public class StagerDao : IStagerDao
    {
        public List<MetricsModel> GetSystemMetricsDataFromWorkflow(string siteId, DateTime from, DateTime to)
        {
            var metricsList = new List<MetricsModel>();
            try
            {
                using (var conn = Database.Instance.ReadConnection)
                {
                    conn.Open();
                    using (var cmdx = conn.CreateCommandEx())
                    {
                        var cmd = cmdx.Cmd;
                        cmd.CommandText = "rtrsapp.pkg_log.SP_GET_WF_COUNT_PER_SECOND";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.BindByName(true);
                        cmd.AddParameter("P_DATE_FROM", ODT.Date, from);
                        cmd.AddParameter("P_DATE_TO", ODT.Date, to);
                        cmd.AddOutputParameter("p_data", ODT.Cursor);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                metricsList.Add(new MetricsModel
                                {
                                    SiteId = siteId,
                                    CompletionSecond = reader["completion_second"] as DateTime?,
                                    WorkflowId = Convert.ToInt16(reader["workflow_id"] as decimal?),
                                    MessageCount = reader["message_count_completed"] as decimal?
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Loggers.ApplicationTrace.Error("Error when getting system metrics data from workflow", ex);
                throw;
            }
            
            return metricsList;
        }

        public virtual void InsertSystemMetricsData(List<MetricsModel> metricsList)
        {
            metricsList.ForEach(InsertSingleMetricsData);
        }

        public virtual void InsertSingleMetricsData(MetricsModel mm)
        {
            try
            {
                using (var conn = Database.Instance.WriteConnection)
                {
                    conn.Open();


                }
            }
            catch (Exception ex)
            {
                Loggers.ApplicationTrace.Error(string.Format("StagerDAO.InsertSingleMetric: Site Id {0}", mm.SiteId), ex);
                throw;
            }
        }
    }
}
