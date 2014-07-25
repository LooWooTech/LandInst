using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    [Table("VApproval_Inst")]
    public class VCheckInst : VCheckBase
    {
        public string InstName { get; set; }

        public InstitutionType Type { get; set; }

        public InstitutionStatus Status { get; set; }

        public string City { get; set; }

        public string LegalRepresentative { get; set; }

        public string MobilePhone { get; set; }

        public string RegistrationNo { get; set; }
    }
}
