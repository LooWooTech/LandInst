using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    /// <summary>
    /// 个人继续教育列表
    /// </summary>
    [Table("VMember_Education")]
    public class VMemberEducation
    {
        [Key]
        public int EduID { get; set; }

        public string EduName { get; set; }
        
        public int? UserID { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Hours { get; set; }

        public string Agency { get; set; }

        public ApprovalType? ApprovalType { get; set; }

        public DateTime? ApprovalTime { get; set; }

        public DateTime? CreateTime { get; set; }

        public bool? Result { get; set; }

        public int ApprovalCount { get; set; }

    }
}
