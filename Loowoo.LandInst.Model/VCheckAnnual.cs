using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    [NotMapped]
    public class VCheckAnnual
    {
        public int AnnualCheckID { get; set; }

        public string AnnualCheckName { get; set; }

        public VCheckInst VCheckInst { get; set; }
    }
}
