using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    [Table("VMemberEducation")]
    public class VMemberEducation
    {
        public int ID { get; set; }

        public int MemberID { get; set; }

        public int EducationID { get; set; }

        public int InstitutionID { get; set; }

        public DateTime SignupTime { get; set; }

        public DateTime? ApprovalTime { get; set; }

        public bool Approval { get; set; }

        public string RealName { get; set; }

        public string EducationName { get; set; }
    }
}
