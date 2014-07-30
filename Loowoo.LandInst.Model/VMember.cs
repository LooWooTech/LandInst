using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    [Table("vmember")]
    public class VMember
    {
        public int ID { get; set; }

        public string Username { get; set; }

        public string RealName { get; set; }

        public DateTime RegisterTime { get; set; }

        //public DateTime? LastLoingTime { get; set; }

        public UserRole Role { get; set; }

        public string InstitutionName { get; set; }

        public int InstitutionID { get; set; }

        public string MobilePhone { get; set; }

        public MemberStatus Status { get; set; }

        public bool Deleted { get; set; }

        public string Gender { get; set; }

        public string Email { get; set; }

        public DateTime? Birthday { get; set; }
    }
}
