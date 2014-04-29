using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loowoo.LandInst.Model;
using Loowoo.LandInst.Model.Filters;

namespace Loowoo.LandInst.Manager
{
    public class InstitutionManager
    {

        public Institution GetInstitution(int userId)
        {
            return new Institution();
        }

        public List<Institution> GetInstitutions(InstitutionFilter filter)
        {
            return new List<Institution>();
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

        public void SaveInstitution(Institution inst)
        {
        }

        public void SaveProfile(InstitutionProfile profile)
        {
        }
    }
}
