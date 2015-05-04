using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    public class Equipment
    {
        public string Name { get; set; }

        public int? Number { get; set; }

        /// <summary>
        /// 型号
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// 生产厂商
        /// </summary>
        public string Manufacturer { get; set; }

        public string Performance { get; set; }

        public string Note { get; set; }

    }
}
