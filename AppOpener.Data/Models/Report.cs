using System;
using System.Collections.Generic;

namespace AppOpener.Data.Models
{
    public partial class Report
    {
        public int ReportId { get; set; }
        public string ReportName { get; set; }
        public int StoredProcedureId { get; set; }
        public int? FileRepositoryId { get; set; }
    }
}
