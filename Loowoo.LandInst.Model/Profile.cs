using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    public class Profile
    {
        public int ID { get; set; }

        public int UserID { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public byte[] Data { get; set; }
    }


}
