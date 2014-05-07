using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loowoo.LandInst.Model.Filters
{
    public class PageFilter
    {
        public PageFilter()
        {
            PageIndex = 1;
            PageSize = 10;
        }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int RecordCount { get; set; }
    }
}
