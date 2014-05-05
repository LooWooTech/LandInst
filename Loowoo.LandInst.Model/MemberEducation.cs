using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loowoo.LandInst.Model
{
    [Table("MemberEducation")]
    public class MemberEducation
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int MemberID { get; set; }

        public int EducationID { get; set; }

        public DateTime SignupTime { get; set; }

        public DateTime? ApprovalTime { get; set; }
        
        public bool Approval { get; set; }

        [NotMapped]
        public string MemberName { get; set; }

        [NotMapped]
        public string InstitutionName { get; set; }

        [NotMapped]
        public string EduName { get; set; }
    }
}
