using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loowoo.LandInst.Model
{
    public class MemberEducation
    {
        public int ID { get; set; }

        public int MemberID { get; set; }

        public int EducationID { get; set; }

        public int CreateTime { get; set; }

        public DateTime ApprovalTime { get; set; }
        
        public bool Approval { get; set; }

        public string MemberName { get; set; }

        //public string InstitutionName { get; set; }

        public string EduName { get; set; }
    }
}
