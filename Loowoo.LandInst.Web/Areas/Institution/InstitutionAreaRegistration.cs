using System.Web.Mvc;

namespace Loowoo.LandInst.Web.Areas.Institution
{
    public class InstitutionAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Institution";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Institution_default",
                "Institution/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "Loowoo.LandInst.Web.Areas.Institution.Controllers" }
            );
        }
    }
}
