using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Feed.UI.MVCApp
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();

            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}