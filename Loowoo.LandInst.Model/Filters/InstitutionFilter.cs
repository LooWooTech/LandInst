using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loowoo.LandInst.Model.Filters
{
    public class InstitutionFilter : PageFilter
    {
        public int? InstId { get; set; }

        public string Keyword { get; set; }

        public InstitutionStatus? Status { get; set; }

        public CheckType ApprovalType { get; set; }

        public bool? ApprovalResult { get; set; }
    }
}
