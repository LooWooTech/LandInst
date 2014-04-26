using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loowoo.LandInst.Model;

namespace Loowoo.LandInst.Manager
{
    public class InstitutionManager
    {

        public Institution GetInsitution(int userId)
        {
            return new Institution();
        }

        public void AddShareholder(Shareholder shareholder)
        { 
            
        }

        public void DeleteShareholder(int shareholderId)
        { 
            
        }

        public void AddCertification(Certification certification)
        {

        }

        public void DeleteCertification(int certificationId)
        {

        }

        public void LogoutInstitution(Institution inst)
        {
        }

        public InstitutionProfile GetProfile(int instId)
        {
            return new InstitutionProfile();
        }
    }
}
