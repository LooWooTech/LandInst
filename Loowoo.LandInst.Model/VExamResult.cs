using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    [Table("VExamResult")]
    public class VExamResult
    {
        public int ID { get; set; }

        public int ExamID { get; set; }

        public string RealName { get; set; }

        public int MemberID { get; set; }

        public string Subjects { get; set; }

        public string Scores { get; set; }

        public int InstitutionID { get; set; }

        public DateTime CreateTime { get; set; }

        [NotMapped]
        public Exam Exam { get; set; }
    }
}
