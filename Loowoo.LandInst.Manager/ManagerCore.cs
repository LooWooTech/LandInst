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

        private MemberManager _memberManager;
        public MemberManager MemberManager
        {
            get { return _memberManager == null ? _memberManager = new MemberManager() : _memberManager; }
        }

        private InstitutionManager _institutionManager;
        public InstitutionManager InstitutionManager
        {
            get { return _institutionManager == null ? _institutionManager = new InstitutionManager() : _institutionManager; }
        }

        private ExamManager _examManager;
        public ExamManager ExamManager
        {
            get { return _examManager == null ? _examManager = new ExamManager() : _examManager; }
        }

    }
}
