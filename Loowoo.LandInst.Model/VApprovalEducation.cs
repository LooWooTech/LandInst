using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    [Table("VApproval_Education")]
    public class VApprovalEducation : VApprovalBase
    {
        public int EduID { get; set; }

        public string EduName { get; set; }

        public string RealName { get; set; }
    }
}
