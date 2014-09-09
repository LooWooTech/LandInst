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
    [Table("Institution")]
    public class Institution
    {
        public Institution()
        {
            CreateTime = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [Column(TypeName = "int")]
        public InstitutionStatus Status { get; set; }

        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 机构名称
        /// </summary>
        [MaxLength(255)]
        public string Name { get; set; }

        /// <summary>
        /// 工商登记号
        /// </summary>
        public string RegistrationNo { get; set; }

        [MaxLength(45)]
        public string City { get; set; }

        /// <summary>
        /// 法人代表
        /// </summary>
        [MaxLength(45)]
        public string LegalPerson { get; set; }

        [MaxLength(45)]
        public string MobilePhone { get; set; }
    }

    [Flags]
    public enum InstitutionStatus
    {
        /// <summary>
        /// 新添加机构状态
        /// </summary>
        [Description("未认证")]
        Normal = 0,

        /// <summary>
        /// 注册登记批准后变更为此状态
        /// </summary>
        [Description("通过认证")]
        Registered = 1,

        /// <summary>
        /// 已注销的机构状态
        /// </summary>
        [Description("注销登记")]
        Logout = 2,
    }
}