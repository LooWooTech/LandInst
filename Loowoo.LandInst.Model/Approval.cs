using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loowoo.LandInst.Model
{
    public class Approval
    {
        public int ID { get; set; }

        public int ProfileID { get; set; }

        public DateTime? CheckTime { get; set; }

        public bool Result { get; set; }

        public string Note { get; set; }

        public ApprovalType Type { get; set; }

        public bool IsLocked
        {
            get
            {
                return !CheckTime.HasValue || Result;
            }
        }
    }

    public enum ApprovalType
    {
        MemberRegister,
        MemeberTransfer,
        InstitutionRegister,
        Education,
        Exam,
        Annual
    }
}
