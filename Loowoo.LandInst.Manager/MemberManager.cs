using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loowoo.LandInst.Model;
using Loowoo.LandInst.Model.Filters;

namespace Loowoo.LandInst.Manager
{
    public class MemberManager
    {
        public void AddMember(User user, Member member)
        {

        }

        public void UpdateMember(Member member)
        { 
            
        }

        public Member GetMember(int userId)
        {
            return new Member
            {
                UserID = userId,
                RealName = "郑良军",

            };
        }


        public MemberProfile GetProfile(int userId)
        {
            return new MemberProfile
            {
                UserID = userId,
                RealName = "郑良军",
            };
        }

        public void SaveProfile(MemberProfile profile)
        {
            throw new NotImplementedException();
        }

        public List<Member> GetMembers(MemberFilter filter)
        {
            return new List<Member>();
        }
    }
}
