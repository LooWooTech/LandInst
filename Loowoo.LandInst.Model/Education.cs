using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loowoo.LandInst.Model
{
    [Table("Education")]
    public class Education
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 学时记录
        /// </summary>
        public string Hours { get; set; }

        public string Agency { get; set; }

        [NotMapped]
        public CheckLog Approval { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new ArgumentException("继续教育名称没有填写");
            }

            if (StartDate == DateTime.MinValue)
            {
                throw new ArgumentException("开始时间没有填写");
            }

            if (StartDate == DateTime.MinValue)
            {
                throw new ArgumentException("结束时间没有填写");
            }

            if (string.IsNullOrEmpty(Hours))
            {
                throw new ArgumentException("学时没有选择");
            }
        }
    }
}