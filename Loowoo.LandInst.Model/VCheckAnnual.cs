using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    [Table("VApproval_Annual")]
    public class VCheckAnnual : VCheckBase
    {
        public int InstID { get; set; }

        public InstitutionType InstType { get; set; }

        public string InstName { get; set; }

        public int AnnualCheckID { get; set; }
    }
}
