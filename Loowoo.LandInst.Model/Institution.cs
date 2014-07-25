﻿using System;
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

        [Column(TypeName = "int")]
        public InstitutionType Type { get; set; }

        /// <summary>
        /// 机构全称
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 机构简称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 工商注册号
        /// </summary>
        public string RegistrationNo { get; set; }

        public string City { get; set; }

        public string MobilePhone { get; set; }

        /// <summary>
        /// 法人代表
        /// </summary>
        public string LegalRepresentative { get; set; }


        [NotMapped]
        public bool HasPassRegisterApproval { get; set; }

        [NotMapped]
        public bool CanSubmitRegisterApproval { get; set; }

        [NotMapped]
        public bool CanSubmitChangeApproval { get; set; }

        //[NotMapped]
        //public bool HasSubmitRegisterApproval { get; set; }

        //[NotMapped]
        //public bool HasSubmitChangeApproval { get; set; }

        //[NotMapped]
        //public bool HasPassChangeApproval { get; set; }
    }

    public enum InstitutionType
    {
        土地规划 = 1,
        土地勘测
    }

    [Flags]
    public enum InstitutionStatus
    {
        /// <summary>
        /// 新添加机构状态
        /// </summary>
        Normal = 0,

        /// <summary>
        /// 注册登记批准后变更为此状态
        /// </summary>
        [Description("注册登记")]
        Register = 1,

        /// <summary>
        /// 变更登记批准后变更为次状态
        /// </summary>
        [Description("变更登记")]
        Change = 2,

        /// <summary>
        /// 已注销的机构状态
        /// </summary>
        [Description("注销登记")]
        Logout = 4,
    }
}
