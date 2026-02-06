using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace NoRiskNoFun.Filters
{
    public class LogSensitiveActivityAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {

            Debug.WriteLine("Sensitive action excuted!!!!!!!");
        }
    }
}
