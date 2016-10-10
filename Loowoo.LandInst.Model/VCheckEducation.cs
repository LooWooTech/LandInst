using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    public class VCheckEducation
    {
        public int EduID { get; set; }

        public string EduName { get; set; }

        public VCheckMember CheckMember { get; set; }
    }
}