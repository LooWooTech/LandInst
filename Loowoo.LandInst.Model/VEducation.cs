using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    [Table("VSelfEducation")]
    public class VSelfEducation
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Hours { get; set; }

        public string Agency { get; set; }

        public DateTime? SignupTime { get; set; }

        public DateTime? ApprovalTime { get; set; }


        public int? MemberID { get; set; }
    }
}
