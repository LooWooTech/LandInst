using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    [Table("VCheck_member")]
    public class VCheckMember : VCheckBase
    {
        public int MemberId { get; set; }

        public string RealName { get; set; }

        public string Gender { get; set; }

        public int InstID { get; set; }

        public string MobilePhone { get; set; }
    }
}