using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loowoo.LandInst.Model.Filters
{
    public class InstitutionFilter
    {
        public int? InstId { get; set; }

        public string Keyword { get; set; }

        public InstitutionStatus? Status { get; set; }

        public CheckType CheckType { get; set; }

        public bool? Result { get; set; }

        public PageFilter Page { get; set; }
    }
}
