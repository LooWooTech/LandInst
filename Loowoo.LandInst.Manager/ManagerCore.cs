using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loowoo.LandInst.Manager
{
    public class ManagerCore
    {
        private UserManager _userManager;
        public UserManager UserManager
        {
            get { return _userManager == null ? _userManager = new UserManager() : _userManager; }
        }

    }
}
