using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    [Table("vcheck_transfer")]
    public class VCheckTransfer : VCheckBase
    {
        public string RealName { get; set; }

        public int TargetInstID { get; set; }

        public TransferMode Mode     { get; set; }

        public int CurrentInstID { get; set; }

        [NotMapped]
        public string CurrentInstName { get; set; }

        [NotMapped]
        public string TargetInstName { get; set; }
    }
}
