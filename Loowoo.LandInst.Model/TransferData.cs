using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    public class TransferData
    {
        public int ApprovalID { get; set; }

        public int MemberID { get; set; }

        public int InstitutionID { get; set; }

        public int TargetInstitutionID { get; set; }

        public TransferMode Mode { get; set; }
    }

    public enum TransferMode
    {
        In, Out
    }
}
