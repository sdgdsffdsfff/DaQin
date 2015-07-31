using System.Web.Mvc;

namespace DaQin.Areas.Auth
{
    public class AuthAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Auth";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Auth_default",
                "Auth/{controller}/{action}/{id}",
                new { area = "Auth", controller = "Account", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
