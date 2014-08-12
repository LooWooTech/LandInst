using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loowoo.LandInst.Model
{
    [NotMapped]
    public class InstitutionProfile : Institution
    {
        public InstitutionProfile()
        {
            ShareHolders = new List<Shareholder>();
            Equipments = new List<Equipment>();
            Softwares = new List<Software>();
        }

        public InstitutionProfile(Institution inst):this()
        {
            SetInstField(inst);
        }

        public void SetInstField(Institution inst)
        {
            foreach (var p in inst.GetType().GetProperties())
            {
                var val = p.GetValue(inst, null);

                var selfP = this.GetType().GetProperty(p.Name);
                selfP.SetValue(this, val, null);
            }
        }

        /// <summary>
        /// 税务登记号
        /// </summary>
        public string TaxRegistryNo { get; set; }

        /// <summary>
        /// 工商登记机关
        /// </summary>
        public string RegistrationInstitution { get; set; }

        /// <summary>
        /// 注册资金
        /// </summary>
        public string RegisteredCapital { get; set; }

        /// <summary>
        /// 经营范围
        /// </summary>
        public string BusinessScope { get; set; }

        /// <summary>
        /// 注册证书号
        /// </summary>
        public string CertificateNo { get; set; }

        /// <summary>
        /// 利税总额
        /// </summary>
        public string TotalProfits { get; set; }

        public string Address { get; set; }

        public string Postcode { get; set; }

        public string Address1 { get; set; }

        public string Postcode1 { get; set; }

        public string Email { get; set; }

        public string Website { get; set; }

        /// <summary>
        /// 法人代码
        /// </summary>
        public string LegalpersonNo { get; set; }

        /// <summary>
        /// 机构类型
        /// </summary>
        public string CompanyType { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string ContactPerson { get; set; }

        /// <summary>
        /// 技术负责人
        /// </summary>
        public string TechLeader { get; set; }

        /// <summary>
        /// 从事土地规划工作办公用房面积
        /// </summary>
        public string OfficeArea { get; set; }

        /// <summary>
        /// 传真电话
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// 营业期限
        /// </summary>
        public string OperatingPeriod { get; set; }

        /// <summary>
        /// 是否有测绘许可证书
        /// </summary>
        public bool HasExequatur { get; set; }

        /// <summary>
        /// 证书级别
        /// </summary>
        public string ExequaturLevel { get; set; }

        /// <summary>
        /// 执业注册号
        /// </summary>
        public string PracticeRegistrationNo { get; set; }

        /// <summary>
        /// 团体会员证号
        /// </summary>
        public string CorporateMemberNo { get; set; }

        /// <summary>
        /// 机构总人数
        /// </summary>
        public int? TotalMembers { get; set; }

        /// <summary>
        /// 其中专业人员数
        /// </summary>
        public int? ProMembers { get; set; }

        /// <summary>
        /// 中级及以上专业人员数
        /// </summary>
        public int? ExpertMembers { get; set; }

        /// <summary>
        /// 成立日期
        /// </summary>
        public DateTime? EstablishedDate { get; set; }

        public List<Shareholder> ShareHolders { get; set; }

        //public List<Certification> Certifications { get; set; }

        public List<Equipment> Equipments { get; set; }

        public List<Software> Softwares { get; set; }
    }
}
