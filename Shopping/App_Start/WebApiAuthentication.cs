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

namespace Shopping
{
    public class WebApiAuthentication : IAuthenticationFilter
    {
        public static readonly string AccessToken = "access_token";
        private readonly ShoppingEntities shoppingEntities;
        private readonly IAuthService authService;

        public WebApiAuthentication()
        {
            shoppingEntities = UnityConfig.GetConfiguredContainer().Resolve<ShoppingEntities>();
            authService = UnityConfig.GetConfiguredContainer().Resolve<IAuthService>();
        }


        public bool AllowMultiple => true;


        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var request = context.Request;

            var publicApis = shoppingEntities.Apis.Where(t => t.Type == Constant.ApiType.PUBLIC).ToList();

            if (publicApis.FirstOrDefault(t => t.Method == request.Method.ToString() 
                && t.Uri == authService.NormalizePath(request.RequestUri.AbsolutePath)) != null)
            {
                return Task.FromResult(0);
            }

            string token = null;

            if (context.Request.Headers.TryGetValues(AccessToken, out var values))
            {
                token = values.First();
            }

            UserToken userToken = shoppingEntities.UserTokens.Where(t => t.Name == token).FirstOrDefault();

            if (userToken == null)
            {
                throw new AuthenticationException("Unauthenticated user");
            }

            return Task.FromResult(0);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}