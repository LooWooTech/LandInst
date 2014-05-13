using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    [Table("VApproval_AnnualCheck")]
    public class VApprovalAnnualCheck : VApprovalBase
    {
        public string Fullname { get; set; }

        public string InstName { get; set; }

        public int AnnualCheckID { get; set; }
    }
}
