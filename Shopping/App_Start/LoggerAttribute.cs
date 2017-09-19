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
            var token = appService.GetTokenFromHeaderHttpRequest(actionExecutedContext);

            Logger logger = new Logger();
            logger.Id = Guid.NewGuid();
            logger.DateTime = System.DateTime.Now;
            logger.ApiMethod = actionExecutedContext.Request.Method.ToString();
            logger.ApiUri = actionExecutedContext.Request.RequestUri.AbsolutePath;
            logger.Success = true;
            logger.Reason = "Successful request";


            if (token == null)
            {
                logger.UserName = "GUEST";
            } 
            else
            {
                var focusToken = shoppingEntities.UserTokens.FirstOrDefault(t => t.Name == token);
                logger.UserName = focusToken.User.Name;
            }

            shoppingEntities.Loggers.Add(logger);
            shoppingEntities.SaveChanges();
        }
    }
}