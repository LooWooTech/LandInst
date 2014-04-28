using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loowoo.LandInst.Model.Filters
{
    public class EducationFilter : PageFilter
    {
        public DateTime? Date { get; set; }

        public int? InstitutionID { get; set; }
    }
}
