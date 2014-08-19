using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loowoo.LandInst.Model
{
    [Table("Exam")]
    public class Exam
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public DateTime StartSignDate { get; set; }

        [Required]
        public DateTime EndSignDate { get; set; }

        [Required]
        public DateTime StartExamDate { get; set; }

        [Required]
        public DateTime EndExamDate { get; set; }

        [Required]
        public string Name { get; set; }

        public string Summary { get; set; }

        public string Address { get; set; }

        public string Subjects { get; set; }
    }
}
