using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model.Filters
{
    public class CheckLogFilter
    {
        public string Keyword { get; set; }

        public bool? Result { get; set; }

        public CheckType Type { get; set; }

        public int? InfoID { get; set; }

        public int? UserID { get; set; }

        public bool? HasCheck { get; set; }

        public DateTime? BeginTime { get; set; }

        public DateTime? EndTime { get; set; }

        public PageFilter Page { get; set; }
    }
}
