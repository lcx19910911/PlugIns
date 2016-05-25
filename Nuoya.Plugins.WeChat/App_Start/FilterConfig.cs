using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Nuoya.Plugins.WeChat.Filters;

namespace Nuoya.Plugins.WeChat
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {         
            filters.Add(new TimerAttribute());          
            filters.Add(new LoginFilterAttribute());

        }
    }
}
