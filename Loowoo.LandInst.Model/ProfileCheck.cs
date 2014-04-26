using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loowoo.LandInst.Model
{
    public class ProfileCheck
    {
        public int ID { get; set; }

        public int ProfileID { get; set; }

        public DateTime? CheckTime { get; set; }

        public bool Pass { get; set; }

        public string Note { get; set; }

        public CheckType Type { get; set; }

        public bool IsLocked
        {
            get
            {
                return !CheckTime.HasValue || Pass;
        }
        }
    }

    public enum CheckType
    {
        Register, Change

    }
}
