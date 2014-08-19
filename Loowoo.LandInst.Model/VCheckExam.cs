using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    public class VCheckExam
    {
        public int ExamID { get { return VCheck.InfoID; } }

        public int MemberID { get { return VCheck.UserID; } }

        public string ExamName { get; set; }

        public VCheckMember VCheck { get; set; }

        public int ID { get { return VCheck.ID; } }

        public string Subjects { get { return VCheck.Data; } }

    }
}
