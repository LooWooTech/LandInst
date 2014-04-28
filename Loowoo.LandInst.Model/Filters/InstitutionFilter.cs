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

        public string LikeName { get; set; }
    }
}
