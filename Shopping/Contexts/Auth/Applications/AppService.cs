using Shopping.App_Start;
using Shopping.Contexts.Auth.Applications.Interfaces;
using Shopping.Ultilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http.Filters;

namespace Shopping.Contexts.Auth.Applications
{
    public class AppService : IAppService
    {

        public string GetTokenFromHeaderHttpRequest(object context) 
        {
            if (context is HttpActionExecutedContext)
            {
            } else if (context is HttpAuthenticationContext)
            {
            } else
            {
                throw new InvalidCastException("Invalid Type!") ;
            }

            string token = null;
            dynamic ctx = context;


            if (ctx.Request.Headers.TryGetValues(RequestConstant.ACCESS_TOKEN, out IEnumerable<string> values))
            {
                token = values.First();
            }

            return null;
        }
    }
}