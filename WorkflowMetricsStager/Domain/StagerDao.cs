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
                                    WorkflowId = reader["workflow_id"] as decimal?,
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

        public void InsertSystemMetricsData(List<MetricsModel> metricsList)
        {
            metricsList.ForEach(InsertSingleMetricsData);
        }

        public void InsertSingleMetricsData(MetricsModel mm)
        {
            try
            {
                using (var conn = Database.Instance.WriteConnection)
                {
                    conn.Open();
                    using (var cmdx = conn.CreateCommandEx())
                    {
                        var cmd = cmdx.Cmd;
                        cmd.CommandText = "RTRSMETRICS.PKG_METRICS.sp_merge_thruput";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.BindByName(true);
                        cmd.AddParameter("P_SITE_ID", ODT.Varchar2, mm.SiteId);
                        cmd.AddParameter("P_COMPLETION_SECOND", ODT.Date, mm.CompletionSecond);
                        cmd.AddParameter("P_WORKFLOW_ID", ODT.Number, mm.WorkflowId);
                        cmd.AddParameter("P_MESSAGE_COUNT_COMPLETED", ODT.Number, mm.MessageCount);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Loggers.ApplicationTrace.Error(string.Format("Error when inserting single metrics data: Site Id {0}, Workflow Id {1} and Completion Second {2}", mm.SiteId, mm.WorkflowId, (Convert.ToDateTime(mm.CompletionSecond)).ToString("yyyy-MM-dd HH:mm:ss")), ex);
                throw;
            }
        }

        public DateTime GetLastRunTime()
        {
            var lastRunTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            try
            {
                using (var conn = Database.Instance.ReadConnection)
                {
                    conn.Open();
                    using (var cmdx = conn.CreateCommandEx())
                    {
                        var cmd = cmdx.Cmd;
                        cmd.CommandText = "RTRSMETRICS.PKG_METRICS.f_get_latest_runtime";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.BindByName(true);
                        var returnValue = cmd.AddParameterEx("Return_Value", null, ODT.Date, ParameterDirection.ReturnValue, 7);

                        cmd.ExecuteScalar();
                        lastRunTime = DateTime.Parse(returnValue.Value.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Loggers.ApplicationTrace.Error(string.Format("Error when getting the workflow stager last run time"), ex);
                throw;
            }

            return lastRunTime;
        }
    }
}
