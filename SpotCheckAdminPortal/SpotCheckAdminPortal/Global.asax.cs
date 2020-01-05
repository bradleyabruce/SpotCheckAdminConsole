using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SpotCheckAdminPortal.Models;

namespace SpotCheckAdminPortal
{
    public class Global : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Session_Start()
        {

        }

        protected void Session_End()
        {
           
        }

    }
}
