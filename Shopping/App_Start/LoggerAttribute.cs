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
        private readonly IAppService appService;

        public LoggerAttribute()
        {
            shoppingEntities = UnityConfig.GetConfiguredContainer().Resolve<ShoppingEntities>();
            authService = UnityConfig.GetConfiguredContainer().Resolve<IAuthService>();
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
                try
                {
                    User user = authService.GetUserInfoFromToken(token).ToModel();
                    logger.UserName = user.Name;

                } catch(Exception e)
                {
                    logger.UserName = "WRONG_TOKEN_USER";
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