using System.Web.Mvc;

namespace Nuoya.Plugins.WeChat.Areas.Puzzle
{
    public class PuzzleAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Puzzle";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Puzzle_default",
                "Puzzle/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}