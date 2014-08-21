using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loowoo.LandInst.Model.Filters
{
    public class InstitutionFilter : CheckLogFilter
    {
        public int? InstId { get; set; }

        public InstitutionStatus? Status { get; set; }

        public string City { get; set; }
    }
}
