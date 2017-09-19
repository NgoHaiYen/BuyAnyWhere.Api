using Shopping.Models;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Microsoft.Practices.Unity;
using Shopping.Contexts.Auth.Applications.Interfaces;
using System;
using System.Linq;

namespace Shopping.App_Start
{
    public class LoggerAttribute : ActionFilterAttribute
    {
        private readonly ShoppingEntities shoppingEntities;
        private readonly IAppService appService;

        public LoggerAttribute()
        {
            shoppingEntities = UnityConfig.GetConfiguredContainer().Resolve<ShoppingEntities>();
            appService = UnityConfig.GetConfiguredContainer().Resolve<IAppService>();
        }


        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var method = actionExecutedContext.Request.Method.ToString();
            var uri = appService.NormalizePath(actionExecutedContext.Request.RequestUri.AbsolutePath);

            var token = appService.GetTokenFromHeaderHttpRequest(actionExecutedContext);

            Logger logger = new Logger();

            logger.Id = Guid.NewGuid();
            logger.DateTime = System.DateTime.Now;
            logger.ApiMethod = method;
            logger.ApiUri = uri;
            logger.Success = true;

            if (token == null)
            {
                logger.UserName = "GUEST";
            } 
            else
            {
                var focusToken = shoppingEntities.UserTokens.FirstOrDefault(t => t.Name == token);

                if (focusToken == null)
                {
                    logger.UserName = "WRONG_TOKEN_USER";

                } else
                {
                    logger.UserName = focusToken.User.Name;
                }
            }

            shoppingEntities.Loggers.Add(logger);
            shoppingEntities.SaveChanges();
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
        }
    }
}