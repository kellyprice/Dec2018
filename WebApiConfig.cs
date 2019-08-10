using System;
using System.Web.Http;

namespace ControlsPortalArchive
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute("DefaultApi",
                "api/{controller}/{action}/{sysid}",
                new { sysid = RouteParameter.Optional });

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.MediaTypeMappings
                .Add(new System.Net.Http.Formatting.RequestHeaderMapping("Accept", "text/html",
                    StringComparison.InvariantCultureIgnoreCase, true, "application/json"));
        }
    }
}
