using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    public class Equipment
    {
        public Equipment()
        {
            ID = Guid.NewGuid().ToString();
        }

        public string ID { get; set; }

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
    }
}
