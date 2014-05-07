using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    [Table("VMember")]
    public class VMember
    {
        public int ID { get; set; }

        public string Username { get; set; }

        public string InstitutionName { get; set; }

        public string RealName { get; set; }

        public MemberStatus Status { get; set; }

        public int InstitutionID { get; set; }

        public string MobilePhone { get; set; }
    }
}
