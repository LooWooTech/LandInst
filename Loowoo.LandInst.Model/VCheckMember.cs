using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    [Table("VCheck_member")]
    public class VCheckMember : VCheckBase
    {
        public string RealName { get; set; }

        public MemberStatus Status { get; set; }

        public string Gender { get; set; }

        public Major Major { get; set; }

        public EduRecord EduRecord { get; set; }

        public int InstitutionID { get; set; }
    }
}
