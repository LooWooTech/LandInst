using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loowoo.LandInst.Web.Areas.Institution.Controllers
{
    public class EducationController : InstitutionControllerBase
    {
        public ActionResult Index(string name, bool? result, int page = 1)
        {
            var filter = new Model.Filters.MemberFilter
            {
                CheckType = Model.CheckType.Education,
                Keyword = name,
                Result = result,
                InstID = Identity.UserID,
                Page = new Model.Filters.PageFilter { PageIndex = page }
            };
            ViewBag.List = Core.EducationManager.GetApprovalEducations(filter);
            ViewBag.PageFilter = filter;
            return View();
        }

        public ActionResult Import()
        {
            ViewBag.Members = Core.MemberManager.GetMembers(new Model.Filters.MemberFilter
            {
                InfoID = Identity.UserID,
            });
            ViewBag.Educations = Core.EducationManager.GetEducations().Where(e => e.EndDate > DateTime.Now).ToList();
            return View();
        }

        public ActionResult Delete(int id)
        {
            Core.CheckLogManager.Delete(id);
            return JsonSuccess();
        }

        [HttpPost]
        public ActionResult Submit(string[] memberNames, string[] memberGenders, string[] memberMobiles, int eduId = 0)
        {
            if (eduId == 0)
            {
                throw new ArgumentException("没有选择具体的继续教育");
            }

            if (memberNames == null || memberGenders == null || memberMobiles == null)
            {
                throw new ArgumentException("请填写参与人员的姓名及其他资料");
            }

            var memberIds = new List<int>();

            for (var i = 0; i < memberNames.Length; i++)
            {
                var memberName = memberNames[i];
                if (string.IsNullOrWhiteSpace(memberName))
                {
                    continue;
                }
                var member = Core.MemberManager.GetMember(memberName, Identity.UserID);
                if (member == null)
                {
                    var memberId = Core.MemberManager.AddMember(new Model.Member
                    {
                        RealName = memberName,
                        Gender = memberGenders[i],
                        MobilePhone = memberMobiles[i],
                        InstitutionID = Identity.UserID
                    });
                    memberIds.Add(memberId);
                }
                else
                {
                    memberIds.Add(member.ID);
                }
            }

            if (memberIds.Count == 0)
            {
                throw new ArgumentException("请输入正确的用户姓名");
            }

            foreach (var id in memberIds)
            {
                Core.EducationManager.SignupEducation(eduId, id, Identity.UserID);
            }
            return JsonSuccess();
        }
    }
}
