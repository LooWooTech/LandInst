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

        public DateTime StartSignTime { get; set; }

        public DateTime EndSignTime { get; set; }

        public DateTime StartExamTime { get; set; }

        public DateTime EndExamTime { get; set; }

        public string Name { get; set; }

        public string Summary { get; set; }
    }
}
