using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orchard.Candidate.Net.Filters
{
    public class CustomActionFilter : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //Add one Id one to the parameters
            filterContext.ActionParameters["id"] = 1;;
            base.OnActionExecuting(filterContext);
        }
    }
}