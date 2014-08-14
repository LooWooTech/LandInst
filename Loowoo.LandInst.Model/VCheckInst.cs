using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    [Table("VCheck_Inst")]
    public class VCheckInst : VCheckBase
    {
        public string InstName { get; set; }

        public InstitutionStatus Status { get; set; }

        public string City { get; set; }

        public string LegalPerson { get; set; }

        public string RegistrationNo { get; set; }
    }
}
