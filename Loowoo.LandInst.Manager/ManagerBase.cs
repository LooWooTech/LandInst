using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loowoo.LandInst.Manager
{
    public class ManagerBase
    {
        protected DBDataContext GetDataContext()
        {
            return new DBDataContext();
        }
    }
}
