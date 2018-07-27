using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Cremcircle.App_Start;
using Cremcircle.Models;

namespace Cremcircle.Models
{
    public static class UrlExtensions
    {
        public static string AuthorizedAction(this UrlHelper url, string action, string controller, object routeValues)
        {
            if (GenFx.IsUserAuthorized(action, controller))
            {
                //Authorized => let him in
                return url.Action(action, controller, routeValues);
            }
            return url.Action("AccessDenied", "Error");
        }

        public static string AuthorizedAction(this UrlHelper url, string action, string controller)
        {
            if (GenFx.IsUserAuthorized(action, controller))
            {
                //Authorized => let him in
                return url.Action(action, controller);
            }
            return url.Action("AccessDenied", "Error");
        }

    }
}