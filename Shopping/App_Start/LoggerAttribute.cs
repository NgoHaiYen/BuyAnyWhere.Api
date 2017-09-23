using System.Net;
using System.Web.Http.Filters;
using Microsoft.Practices.Unity;
using Shopping.App_Start;
using Shopping.Contexts.Auth.Applications.Interfaces;

namespace Shopping
{
    public class LoggerAttribute : ActionFilterAttribute
    {
        private readonly IUltilityService ultilityService;

        public LoggerAttribute()
        {
            ultilityService = UnityConfig.GetConfiguredContainer().Resolve<IUltilityService>();
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Response != null)
            {
                if (actionExecutedContext.Response.StatusCode == HttpStatusCode.OK)
                {
                    var token = ultilityService.GetHeaderToken(actionExecutedContext);
                    ultilityService.Log(actionExecutedContext, token, true, "Thực hiện yêu cầu thành công");
                }
            } 
        }
    }
}