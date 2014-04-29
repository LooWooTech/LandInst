using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loowoo.LandInst.Model
{
    public class Education
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        /// <summary>
        /// 学习内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 学时记录
        /// </summary>
        public string Hours { get; set; }

        public string Agency { get; set; }


    }
}
