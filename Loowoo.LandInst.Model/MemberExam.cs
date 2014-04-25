using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loowoo.LandInst.Model
{
    public class MemberExam
    {
        public MemberExam()
        {
            SignTime = DateTime.Now;
        }

        public int UserID { get; set; }

        public int ExamID { get; set; }

        public string ExamName { get; set; }

        public DateTime SignTime { get; set; }

        public DateTime? ApprovalTime { get; set; }

        public bool Approvaled { get; set; }

        public int ExamResult { get; set; }
    }
}
