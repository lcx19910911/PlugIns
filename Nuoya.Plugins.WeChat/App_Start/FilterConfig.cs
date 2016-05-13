using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Nuoya.Plugins.WeChat.Filters;
using Microsoft.Practices.Unity;

namespace Nuoya.Plugins.WeChat
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {         
            filters.Add(new TimerAttribute());          
            var loginFilter = App_Start.UnityConfig.GetConfiguredContainer().Resolve<LoginFilterAttribute>();
            filters.Add(loginFilter);

        }
    }
}
