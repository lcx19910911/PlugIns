using System.Web.Mvc;

namespace Nuoya.Plugins.WeChat.Areas.scratchcard
{
    public class scratchcardAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Scratchcard";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Scratchcard_default",
                "Scratchcard/{controller}/{action}/{id}",
                new { action = "Index", controller = "Shome", id = UrlParameter.Optional }
            );
        }
    }
}