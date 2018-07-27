using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Cremcircle.Models;
using Cremcircle.App_Start;
namespace Cremcircle
{
    public static class LinkExtensions
    {
        public static MvcHtmlString Authorized(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (GenFx.IsUserAuthorized(actionName, controllerName))
                {
                    //Authorized => let him in
                    return htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
                }
                return MvcHtmlString.Empty;
            }
            return MvcHtmlString.Empty;
        }

        
    }
}