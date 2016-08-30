using System;
using System.Data;
using Emma.Config;
using RTRSCommon;
using RTRSOpDashboard.DataModel;

namespace MQMetricsStager.Domain
{
    public interface IStagerDao
    {
        void InsertMqCurrentDepth(MetricsModel mm);
    }

    public class StagerDao : IStagerDao
    {
        public void InsertMqCurrentDepth(MetricsModel mm)
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
                Loggers.ApplicationTrace.Error(string.Format("Error when inserting mq current depth: Site Id {0}, Workflow Id {1} and Completion Second {2}", mm.SiteId, mm.WorkflowId, (Convert.ToDateTime(mm.CompletionSecond)).ToString("yyyy-MM-dd HH:mm:ss")), ex);
                throw;
            }
        }
    }
}
