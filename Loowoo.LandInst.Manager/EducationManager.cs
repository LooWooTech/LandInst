using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loowoo.LandInst.Model;
using Loowoo.LandInst.Model.Filters;

namespace Loowoo.LandInst.Manager
{
    public class EducationManager
    {
        public void AddEducation(Education education)
        {

        }

        public void CheckEducation(int memberId)
        {

        }

        public List<Education> GetEducations(EducationFilter filter = null)
        {
            return new List<Education>();
        }

        public Education GetEducatoin(int eduId)
        {
            return new Education();
        }

        public void SaveEducation(Education education)
        {

        }

        public List<MemberEducation> GetMemberEducations(EducationFilter filter)
        {
            return new List<MemberEducation>() { new MemberEducation { EducationID = 1, MemberID = 1, MemberName = "Test", EduName = "test" } };
        }

        public void Approval(int memberId, int eduId, bool result)
        {

        }
    }
}
