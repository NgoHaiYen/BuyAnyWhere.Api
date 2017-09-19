using Shopping.App_Start;
using Shopping.Contexts.Auth.Applications.Interfaces;
using Shopping.Models;
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

        private readonly ShoppingEntities shoppingEntities;

        public AppService(ShoppingEntities shoppingEntities)
        {
            this.shoppingEntities = shoppingEntities;
        }

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


        public void Log(string apiMethod, string userToken, string apiUri, bool success, string reason)
        {
            return;
        }


        public string NormalizePath(string path)
        {
            string[] linkParts = path.Split('/').Where(x => !string.IsNullOrEmpty(x)).ToArray();

            for (int i = 0; i < linkParts.Length; i++)
            {
                Guid testGuid;
                if (Guid.TryParse(linkParts[i], out testGuid))
                {
                    linkParts[i] = "*";
                }
            }
            return String.Join("/", linkParts);
        }
    }
}