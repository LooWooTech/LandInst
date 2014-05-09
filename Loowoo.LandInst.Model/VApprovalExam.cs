﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    [Table("VApproval_Exam")]
    public class VApprovalExam : VApprovalBase
    {
        public int ExamID { get; set; }

        public string ExamName { get; set; }

        public string RealName { get; set; }
    }
}
