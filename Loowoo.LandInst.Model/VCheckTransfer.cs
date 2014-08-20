using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Model
{
    public class VCheckTransfer
    {
        public int ID
        {
            get
            {
                return VCheck.ID;
            }
        }

        public int TargetInstID
        {
            get
            {
                return int.Parse(VCheck.Data);
            }
        }

        public int CurrentInstID
        {
            get
            {
                return VCheck.InfoID;
            }
        }

        public VCheckMember VCheck { get; set; }

        public string CurrentInstName { get; set; }

        public string TargetInstName { get; set; }
    }
}
