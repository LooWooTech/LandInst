using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loowoo.LandInst.Model
{
    public class Exam
    {
        public int ID { get; set; }

        public DateTime StartSignTime { get; set; }

        public DateTime EndSignTime { get; set; }

        public DateTime StartExamTime { get; set; }

        public DateTime EndExamTime { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }
    }
}
