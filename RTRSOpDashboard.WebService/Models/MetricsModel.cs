using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RTRSOpDashboard.WebService.Models
{
    public class MetricsModel
    {
        public string SiteId { get; set; }
        public DateTime? CompletionSecond { get; set; }
        public short? WorkflowId { get; set; }
        public decimal? MessageCount { get; set; }
    }
}