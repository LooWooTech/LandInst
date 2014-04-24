using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loowoo.LandInst.Model;
using Loowoo.LandInst.Model.Filters;

namespace Loowoo.LandInst.Manager
{
    public class ExamManager
    {
        public List<Exam> GetExams(ExamFilter filter = null)
        {

            return new List<Exam> { new Exam { ID = 1, Name = "测试考试" } };
        }
    }
}
