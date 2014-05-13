using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    public class VApprovalBase
    {
        public int ID { get; set; }

        public int UserID { get; set; }

        public int InfoID { get; set; }
        
        public ApprovalType ApprovalType { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? ApprovalTime { get; set; }

        public bool? Result { get; set; }

    }
}
