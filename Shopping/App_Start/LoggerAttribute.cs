using Shopping.Models;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Microsoft.Practices.Unity;
using System.Linq;
using Shopping.Contexts.Auth.Applications.Interfaces;
using Shopping.Ultilities;
using System;

namespace Shopping.App_Start
{
    public class LoggerAttribute : ActionFilterAttribute
    {
        private readonly ShoppingEntities shoppingEntities;
        private readonly IAuthService authService;

        public LoggerAttribute()
        {
            shoppingEntities = UnityConfig.GetConfiguredContainer().Resolve<ShoppingEntities>();
            this.authService = UnityConfig.GetConfiguredContainer().Resolve<IAuthService>();
        }


        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var method = actionExecutedContext.Request.Method.ToString();
            var uri = actionExecutedContext.Request.RequestUri.AbsolutePath;

            var api = shoppingEntities.Apis.FirstOrDefault(t => t.Method == method && t.Uri == uri);

            string token = null;

            if (actionExecutedContext.Request.Headers.TryGetValues(RequestConstant.ACCESS_TOKEN, out var values))
            {
                token = values.First();
            }

            var user = authService.GetUserInfoFromToken(token).ToModel();

            Logger logger = new Logger();

            logger.Id = Guid.NewGuid();
            logger.User = user;
            logger.Api = api;

            shoppingEntities.Loggers.Add(logger);

            shoppingEntities.SaveChanges();
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
        }
    }
}