using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    [Table("Member")]
    public class Member
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int InstID { get; set; }

        public string RealName { get; set; }

        public string IDNo { get; set; }

        public string Birthday { get; set; }

        public string CertificationNo { get; set; }

        public string PracticeNo { get; set; }

        public string MobilePhone { get; set; }

        public string Gender { get; set; }

        public static List<Member> GetList(NameValueCollection requestForm)
        {
            var list = new List<Member>();
            if (!requestForm.AllKeys.Contains("member.RealName"))
            {
                return list;
            }
            var realNames = requestForm["member.RealName"].Split(',');
            var genders = requestForm["member.Gender"].Split(',');
            var birthdays = requestForm["member.Birthday"].Split(',');
            var practiceNos = requestForm["member.PracticeNo"].Split(',');
            //var certificationNos = requestForm["member.CertificationNo"].Split(',');
            var mobilePhones = requestForm["member.MobilePhone"].Split(',');
            var idNos = requestForm["member.IDNo"].Split(',');

            for (var i = 0; i < realNames.Length; i++)
            {
                var realName = realNames[i];
                var idNo = idNos[i];

                if (string.IsNullOrEmpty(realName) || string.IsNullOrEmpty(idNo))
                {
                    throw new ArgumentException("请填写姓名和身份证号码");
                }

                list.Add(new Member
                {
                    RealName = realName,
                    Gender = genders[i],
                    Birthday = birthdays[i],
                    //CertificationNo = certificationNos[i],
                    PracticeNo = practiceNos[i],
                    MobilePhone = mobilePhones[i],
                    IDNo = idNo,
                });
            }
            return list;
        }
    }
}
