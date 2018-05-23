using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Mvc;

namespace DotNetAppSqlDb
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { controller="Temperatures", id = UrlParameter.Optional }
            );
            config.Formatters.JsonFormatter
                .SupportedMediaTypes.Add(new MediaTypeHeaderValue("multipart/form-data"));
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            //config.MessageHandlers.Add(new AuthenticationTestHandler());
            
            //config.Filters.Add(new IdentityBasicAuthentication());
        }
    }
}
