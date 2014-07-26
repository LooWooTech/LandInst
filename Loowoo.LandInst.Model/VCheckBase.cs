using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    public class VCheckBase
    {
        public int ID { get; set; }

        public int InfoID { get; set; }

        public int UserID { get; set; }
        
        public CheckType CheckType { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public bool? Result { get; set; }

    }
}
