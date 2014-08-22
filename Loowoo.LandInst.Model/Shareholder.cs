using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Loowoo.LandInst.Model
{
    public class Shareholder
    {
        public string Name { get; set; }

        public string Gender { get; set; }

        public string Birthday { get; set; }

        public string Shares { get; set; }

        public string Mobile { get; set; }

        public string Title { get; set; }

        public bool? Professionals { get; set; }

        public static List<Shareholder> GetList(NameValueCollection requestForm)
        {
            var list = new List<Shareholder>();
            try
            {
                var shNames = requestForm["SH.Name"].Split(',');
                var shGenders = requestForm["SH.Gender"].Split(',');
                var shBirthdays = requestForm["SH.Birthday"].Split(',');
                var shShares = requestForm["SH.Shares"].Split(',');
                var shTitles = requestForm["SH.Title"].Split(',');
                var shProfessionals = requestForm["SH.Professionals"].Split(',');

                for (var i = 0; i < shNames.Length; i++)
                {
                    list.Add(new Shareholder
                    {
                        Name = shNames[i],
                        Gender = shGenders[i],
                        Birthday = shBirthdays[i],
                        Shares = shShares[i],
                        Title = shTitles[i],
                        Professionals = shProfessionals[i] == "是"
                    });
                }
            }
            catch { }
            return list;
        }
    }
}