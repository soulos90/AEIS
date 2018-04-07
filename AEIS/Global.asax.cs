using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Http;

namespace StateTemplateV5Beta
{
    
    public class MvcApplication : System.Web.HttpApplication
    {
        public static Controllers.SecurityController active;
        public static Controllers.EnvironmentController environment;
        protected void Application_Start()
        {
            active = new Controllers.SecurityController();
            environment = new Controllers.EnvironmentController();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            
        }
    }
}
