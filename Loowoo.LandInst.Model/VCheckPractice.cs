using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    [Table("vcheck_practice")]
    public class VCheckPractice : VCheckBase
    {
        public string RealName { get; set; }

        public int InstitutionID { get; set; }

        public string MobilePhone { get; set; }

        public string Email { get; set; }

        public MemberStatus Status { get; set; }
    }
}
