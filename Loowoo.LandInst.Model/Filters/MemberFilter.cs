using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loowoo.LandInst.Model.Filters
{
    public class MemberFilter : CheckLogFilter
    {
        public MemberFilter()
        {
            InInst = true;
        }

        public int[] MemberIds { get; set; }

        public int? InstID { get; set; }

        public bool InInst { get; set; }

        public bool IncludeNoHaveInstMember { get; set; }

        public bool GetInstName { get; set; }
    }
}
