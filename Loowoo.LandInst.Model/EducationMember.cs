using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    /// <summary>
    /// 申请继续教育的会员资料（新的需求，和Member没关系）
    /// </summary>
    [Table("education_member")]
    public class EducationMember
    {
        public EducationMember()
        {
            CreateTime = DateTime.Now;
        }

        [Key]
        public int ID { get; set; }

        public int EducationID { get; set; }

        public int InstitutionID { get; set; }

        public string Name { get; set; }

        public string Gender { get; set; }

        public string Mobile { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
