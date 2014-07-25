using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loowoo.LandInst.Web.Areas.Institution
{
    public class InstitutionControllerBase : ControllerBase
    {
        protected Model.Institution GetCurrentInst()
        {
            return Session[sessionKey] as Model.Institution;
        }

        private string sessionKey = "currentInst";
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session[sessionKey] == null)
            {
                var inst = Core.InstitutionManager.GetInstitution(Identity.UserID);
                var registerApproval = Core.ApprovalManager.GetApproval(inst.ID, Identity.UserID, Model.ApprovalType.Register);
                //inst.HasSubmitRegisterApproval = registerApproval != null || (registerApproval != null && registerApproval.Result == false);
                inst.CanSubmitRegisterApproval = registerApproval == null || registerApproval.Result == false;
                inst.HasPassRegisterApproval = registerApproval != null && registerApproval.Result.HasValue && registerApproval.Result.Value;

                var changeApproval = Core.ApprovalManager.GetApproval(inst.ID, Identity.UserID, Model.ApprovalType.Change);
                inst.CanSubmitChangeApproval = (changeApproval != null && changeApproval.Result.HasValue) || changeApproval == null;
                //inst.HasSubmitChangeApproval = changeApproval != null;
                //inst.HasPassChangeApproval = changeApproval != null && changeApproval.Result.Value;

                Session[sessionKey] = inst;
            }
            ViewBag.CurrentInst = Session[sessionKey];
            base.OnActionExecuting(filterContext);
        }

    }
}
