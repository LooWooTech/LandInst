﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    [Table("InfoData")]
    public class InfoData
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int InfoID { get; set; }

        [Column(TypeName = "int")]
        public InfoType InfoType { get; set; }

        public byte[] Data { get; set; }

        [Column(TypeName = "int")]
        public InfoStatus Status { get; set; }
    }

    public enum InfoType
    {
        MemberProfile = 1,
        InstitutionProfile = 2,
        Shareholder,
        Certificatoin,
        Exam
    }

    [Flags]
    public enum InfoStatus
    {
        Normal = 1,
        Draft = 2,
    }
}