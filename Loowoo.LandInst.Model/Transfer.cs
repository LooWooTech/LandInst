using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    //[Table("transfer")]
    //public class Transfer
    //{
    //    public Transfer()
    //    {
    //        CreateTime = DateTime.Now;
    //    }

    //    [Key]
    //    [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
    //    public int ID { get; set; }

    //    public int MemberID { get; set; }

    //    public int CurrentInstID { get; set; }

    //    public int TargetInstID { get; set; }

    //    public TransferMode Mode { get; set; }

    //    public DateTime CreateTime { get; set; }

    //    public DateTime? UpdateTime { get; set; }
    //}

    public enum TransferMode
    {
        [Description("转入")]
        In, 
        [Description("转出")]
        Out
    }
}
