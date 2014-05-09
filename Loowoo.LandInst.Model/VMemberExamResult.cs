using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    /// <summary>
    /// 个人成绩查询列表
    /// </summary>
    [Table("VMember_ExamResult")]
    public class VMemberExamResult
    {
        [Key]
        public int ID { get; set; }

        public int MemberID { get; set; }

        public int ExamID { get; set; }

        public bool Result { get; set; }

        public string ExamName { get; set; }
    }
}
