namespace WebApi.ActionFilters
{
    using System;
    using System.Web.Http.Filters;
    using System.Web.Http.Controllers;
    using System.Web.Http.Tracing;
    using System.Web.Http;
    using Helpers;
    using System.Threading;
    using System.Threading.Tasks;

    public class LoggingFilterAttribute : ActionFilterAttribute
    {
        public override Task OnActionExecutingAsync(HttpActionContext filterContext, CancellationToken cancellationToken)
        {
            GlobalConfiguration.Configuration.Services.Replace(typeof(ITraceWriter), new NLogger());
            var trace = GlobalConfiguration.Configuration.Services.GetTraceWriter();
            trace.Info(filterContext.Request, "Controller : " + filterContext.ControllerContext.ControllerDescriptor.ControllerType.FullName + Environment.NewLine + "Action : " + filterContext.ActionDescriptor.ActionName, "JSON", filterContext.ActionArguments);
            return base.OnActionExecutingAsync(filterContext, cancellationToken);   
        }
        //public override void OnActionExecuting(HttpActionContext filterContext)
        //{
        //    GlobalConfiguration.Configuration.Services.Replace(typeof(ITraceWriter), new NLogger());
        //    var trace = GlobalConfiguration.Configuration.Services.GetTraceWriter();
        //    trace.Info(filterContext.Request, "Controller : " + filterContext.ControllerContext.ControllerDescriptor.ControllerType.FullName + Environment.NewLine + "Action : " + filterContext.ActionDescriptor.ActionName, "JSON", filterContext.ActionArguments);
        //}

        //public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        //{
        //    GlobalConfiguration.Configuration.Services.Replace(typeof(ITraceWriter), new NLogger());
        //    var trace = GlobalConfiguration.Configuration.Services.GetTraceWriter();
        //   // object response = JSONHelper.ToObject(actionExecutedContext.Response.Content.ReadAsStringAsync().Result);
        //    trace.Info(actionExecutedContext.Request, "Response : " + actionExecutedContext.Response.Content.ReadAsStringAsync().Result, "JSON", "data retrieved");
        //    return base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);
        //}
    }
}