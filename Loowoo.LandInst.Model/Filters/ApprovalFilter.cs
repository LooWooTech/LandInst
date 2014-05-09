using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model.Filters
{
    public class ApprovalFilter : PageFilter
    {
        public bool? Result { get; set; }

        public ApprovalType Type { get; set; }

        public int? InfoID { get; set; }

        public DateTime? BeginTime { get; set; }

        public DateTime? EndTime { get; set; }
    }
}
