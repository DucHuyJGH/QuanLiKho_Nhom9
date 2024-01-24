using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QLK.Website
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);


        }
        protected void Application_Error()
        {
            Exception unhandledException = Server.GetLastError();
            HttpException httpException = unhandledException as HttpException;
            if (httpException == null)
            {
                Exception innerException = unhandledException.InnerException;
                httpException = innerException as HttpException;
            }

            if (httpException != null)
            {
                int httpCode = httpException.GetHttpCode();
                switch (httpCode)
                {
                    case (int)HttpStatusCode.Unauthorized:
                        Response.Redirect("/Http/Error401");
                        break;
                }
            }
        }

    }
}
