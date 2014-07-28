using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    public class VCheckExam
    {
        public int ExamID { get; set; }

        public string ExamName { get; set; }

        public VCheckMember Member { get; set; }
    }
}
