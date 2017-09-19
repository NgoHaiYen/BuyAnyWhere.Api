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
        private readonly IUltilityService ultilityService;

        public LoggerAttribute()
        {
            shoppingEntities = UnityConfig.GetConfiguredContainer().Resolve<ShoppingEntities>();
            ultilityService = UnityConfig.GetConfiguredContainer().Resolve<IUltilityService>();
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var token = ultilityService.GetTokenFromHeaderHttpRequest(actionExecutedContext);

            Logger logger = new Logger();
            logger.Id = Guid.NewGuid();
            logger.DateTime = System.DateTime.Now;
            logger.ApiMethod = actionExecutedContext.Request.Method.ToString();
            logger.ApiUri = actionExecutedContext.Request.RequestUri.AbsolutePath;
            logger.Success = true;
            logger.Reason = "Successful request";
            logger.UserName = ultilityService.GetUserFromTokenAlwayReturnUserName(token).Name;


            shoppingEntities.Loggers.Add(logger);
            shoppingEntities.SaveChanges();
        }
    }
}