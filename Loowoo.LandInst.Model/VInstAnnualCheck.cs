using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    [Table("VInst_AnnualCheck")]
    public class VInstAnnualCheck
    {
        [Key]
        public int ID { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Name { get; set; }

        public int? UserID { get; set; }

        public ApprovalType? ApprovalType { get; set; }

        public DateTime? ApprovalTime { get; set; }

        public bool? Result { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
