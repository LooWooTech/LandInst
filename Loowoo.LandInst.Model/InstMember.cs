using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    public class InstMember
    {
        public string Name { get; set; }

        public string Birthday { get; set; }

        public string CertificationNo { get; set; }

        public string PracticeNo { get; set; }

        public string MobilePhone { get; set; }

        public string Gender { get; set; }

        public static List<InstMember> GetList(NameValueCollection requestForm)
        {
            var list = new List<InstMember>();
            try
            {
                var memberNames = requestForm["member.Name"].Split(',');
                var memberGenders = requestForm["member.Gender"].Split(',');
                var memberBirthdays = requestForm["member.Birthday"].Split(',');
                var memberPracticeNos = requestForm["member.PracticeNo"].Split(',');
                var memberMobiles = requestForm["member.MobilePhone"].Split(',');

                for (var i = 0; i < memberNames.Length; i++)
                {
                    list.Add(new InstMember
                    {
                        Name = memberNames[i],
                        Gender = memberGenders[i],
                        Birthday = memberBirthdays[i],
                        PracticeNo = memberPracticeNos[i],
                        MobilePhone = memberMobiles[i]
                    });
                }
            }
            catch { }
            return list;
        }
    }
}
