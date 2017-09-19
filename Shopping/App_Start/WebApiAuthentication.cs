using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using Microsoft.Practices.Unity;
using Shopping.App_Start;
using Shopping.Models;
using System.Security.Authentication;
using System.Collections.Generic;
using Shopping.Contexts.Auth.Services;
using Shopping.Contexts.Auth.Applications.Interfaces;
using Shopping.Ultilities;
using System;

namespace Shopping
{
    public class WebApiAuthentication : IAuthenticationFilter
    {

        private readonly ShoppingEntities shoppingEntities;
        private readonly IAppService appService;

        public WebApiAuthentication()
        {
            shoppingEntities = UnityConfig.GetConfiguredContainer().Resolve<ShoppingEntities>();
            appService = UnityConfig.GetConfiguredContainer().Resolve<IAppService>();
        }


        public bool AllowMultiple => true;


        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var request = context.Request;
            var publicApis = shoppingEntities.Apis.Where(t => t.Type == ApiTypeConstant.PUBLIC).ToList();

            // Neu la public Api
            if (publicApis.FirstOrDefault(t => t.Method == request.Method.ToString() 
                && t.Uri == appService.NormalizePath(request.RequestUri.AbsolutePath)) != null)
            {
                return Task.FromResult(0);
            }


            // Neu la private Api

            string token = appService.GetTokenFromHeaderHttpRequest(context);
            UserToken userToken = shoppingEntities.UserTokens.Where(t => t.Name == token).FirstOrDefault();

            if (userToken == null)
            {
                Logger logger = new Logger();
                logger.Id = Guid.NewGuid();
                logger.UserName = "GUEST";
                logger.ApiMethod = request.Method.ToString();
                logger.DateTime = System.DateTime.Now;
                logger.ApiUri = request.RequestUri.AbsolutePath;
                logger.Success = false;
                logger.Reason = "User must login to access this private api";

                shoppingEntities.Loggers.Add(logger);
                shoppingEntities.SaveChanges();

                throw new AuthenticationException("Unauthenticated user");
            } 
            // Neu token het han

            return Task.FromResult(0);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}