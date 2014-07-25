using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    public class Transfer
    {
        public int ID { get; set; }

        public int MemberID { get; set; }

        public int CurrentInstID { get; set; }

        public int TargetInstID { get; set; }

        public TransferMode Mode { get; set; }
    }

    public enum TransferMode
    {
        In, Out
    }
}
