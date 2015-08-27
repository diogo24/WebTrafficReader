using System.Web;
using System.Web.Mvc;

namespace WebTrafficAnalyser
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
