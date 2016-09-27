using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTRSOpDashboard.DataModel
{
    public class MetricsModel
    {
        public string SiteId { get; set; }
        public DateTime? CompletionSecond { get; set; }
<<<<<<< HEAD
        public decimal? WorkflowId { get; set; }
=======
        public short? WorkflowId { get; set; }
>>>>>>> e0e1ff74d1444cc92b0949760451e6c7a2b14f0a
        public decimal? MessageCount { get; set; }
    }
}
