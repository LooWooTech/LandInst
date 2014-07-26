using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loowoo.LandInst.Model.Filters
{
    public class MemberFilter : CheckLogFilter
    {
        public int[] MemberIds { get; set; }

        public int? InstID { get; set; }

        public MemberStatus? Status { get; set; }
    }
}
