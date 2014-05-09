using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    /// <summary>
    /// 个人考试报名列表
    /// </summary>
    [Table("VMember_Exam")]
    public class VMemberExam
    {
        [Key]
        public int ExamID { get; set; }

        public int MemberID { get; set; }

        public DateTime StartSignTime { get; set; }

        public DateTime EndSignTime { get; set; }

        public DateTime StartExamTime { get; set; }

        public DateTime EndExamTime { get; set; }

        public string ExamName { get; set; }

        public bool? Result { get; set; }

        public DateTime? ApprovalTime { get; set; }

        public DateTime SignTime { get; set; }

        public ApprovalType ApprovalType { get; set; }
    }
}
