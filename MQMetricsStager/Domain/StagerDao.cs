using System;
<<<<<<< HEAD
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
=======
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQMetricsStager.Domain
{
    public class StagerDao
    {

>>>>>>> e0e1ff74d1444cc92b0949760451e6c7a2b14f0a
    }
}
