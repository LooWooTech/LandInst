﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    [Table("ExamResult")]
    public class ExamResult
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int ExamID { get; set; }

        public int MemberID { get; set; }

        public bool Result { get; set; }

        [NotMapped]
        public string ExamName { get; set; }
    }
}
