﻿using Shopping.Models;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Microsoft.Practices.Unity;
using Shopping.Contexts.Auth.Applications.Interfaces;
using System;
using System.Linq;
using System.Net;

namespace Shopping.App_Start
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
                    var token = ultilityService.GetTokenFromHeaderHttpRequest(actionExecutedContext);
                    ultilityService.Log(actionExecutedContext, token, true, "Successful Request");
                }
            } 
        }
    }
}