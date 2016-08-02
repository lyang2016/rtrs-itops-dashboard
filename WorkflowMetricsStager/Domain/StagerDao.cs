using System;
using System.Collections.Generic;
using Emma.Config;
using RTRSCommon;
using RTRSOpDashboard.DataModel;

namespace WorkflowMetricsStager.Domain
{
    public class StagerDao : IStagerDao
    {
        public List<MetricsModel> GetSystemMetricsData()
        {
            var metricsList = new List<MetricsModel>();
            /*
            try
            {
                using (var conn = Database.Instance.ReadConnection)
                {
                    conn.Open();
                    using (var cmdx = conn.CreateCommandEx())
                    {
                        var cmd = cmdx.Cmd;
                        cmd.CommandText = "rtrsapp.pkg_rtrs.sp_get_messages_to_parse_v3";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.BindByName(true);
                        cmd.AddParameter("p_no_message", ODT.Number, noOfMsg);
                        cmd.AddParameter("p_lock_second", ODT.Number, secondsToLock);
                        cmd.AddOutputParameter("p_data", ODT.Cursor);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                metricsList.Add(new MetricsModel
                                {
                                    SiteId = reader["SITE_ID"] as string,
                                    CompletionSecond = reader["COMPLETION_SECOND"] as DateTime?,
                                    WorkflowId = reader["WORKFLOW_ID"] as short?,
                                    MessageCount = reader["MESSAGE_COUNT_COMPLETED"] as decimal?
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Loggers.ApplicationTrace.Error("Error when pulling message to parse", ex);
                throw;
            }
             */
            return metricsList;
        }

        public virtual void InsertSystemMetricsData(List<MetricsModel> metricsList)
        {
            metricsList.ForEach(InsertSingleMetric);
        }

        public virtual void InsertSingleMetric(MetricsModel mm)
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
