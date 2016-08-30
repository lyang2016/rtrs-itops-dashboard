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
        public decimal? WorkflowId { get; set; }
        public decimal? MessageCount { get; set; }
    }
}
