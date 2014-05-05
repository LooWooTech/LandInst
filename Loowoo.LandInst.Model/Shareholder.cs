using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loowoo.LandInst.Model
{
    public class Shareholder
    {
        public Shareholder()
        {
            ID = Guid.NewGuid().ToString();
        }

        public string ID { get; set; }

        public string Name { get; set; }

        public string Gender { get; set; }

        public string Birthday { get; set; }

        public string Shares { get; set; }

        public string Mobile { get; set; }
    }
}
