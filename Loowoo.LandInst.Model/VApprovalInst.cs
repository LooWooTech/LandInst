﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    [Table("VApproval_Inst")]
    public class VApprovalInst : VApprovalBase
    {
        public string InstName { get; set; }

        public string FullName { get; set; }

        public InstitutionType Type { get; set; }

        public InstitutionStatus Status { get; set; }

        public string City { get; set; }

        public string LegalRepresentative { get; set; }

        public string MobilePhone { get; set; }
    }
}
