using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    [Table("Vapproval_member")]
    public class VCheckMember : VCheckBase
    {
        public string Username { get; set; }

        public string RealName { get; set; }

        public MemberStatus Status { get; set; }

        public int Gender { get; set; }

        public string MobilePhone { get; set; }

        public int InstitutionID { get; set; }
    }
}
