using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loowoo.LandInst.Model
{
    public class MemberExam
    {
        public int UserID { get; set; }

        public int ExamID { get; set; }

        public DateTime SignTime { get; set; }

        public DateTime? ApprovalTime { get; set; }

        public bool SignStatus { get; set; }

        public int ExamResult { get; set; }
    }
}
