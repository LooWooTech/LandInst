using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loowoo.LandInst.Model
{
    [Table("CheckLog")]
    public class CheckLog
    {
        public CheckLog()
        {
            CreateTime = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int UserID { get; set; }

        public int InfoID { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public bool? Result { get; set; }

        public string Note { get; set; }

        [Column(TypeName = "int")]
        public CheckType CheckType { get; set; }

        [NotMapped]
        public bool Checked { get { return Result.HasValue; } }
    }

    public enum CheckType
    {
        [Description("注册/变更登记")]
        Profile = 1,
        [Description("会员转移")]
        Transfer,
        [Description("执业登记")]
        Working,
        [Description("年检审核")]
        Annual,
        [Description("继续教育")]
        Education,
        [Description("报名考试")]
        Exam
    }
}
