using System.Web;
using System.Web.Mvc;

namespace TwitterApiClient.ApiCallback
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}