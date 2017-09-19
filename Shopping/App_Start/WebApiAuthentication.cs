using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using Microsoft.Practices.Unity;
using Shopping.App_Start;
using Shopping.Models;
using System.Security.Authentication;
using Shopping.Contexts.Auth.Applications.Interfaces;
using Shopping.Ultilities;

namespace Shopping
{
    public class WebApiAuthentication : IAuthenticationFilter
    {

        private readonly ShoppingEntities shoppingEntities;
        private readonly IUltilityService appService;

        public WebApiAuthentication()
        {
            shoppingEntities = UnityConfig.GetConfiguredContainer().Resolve<ShoppingEntities>();
            appService = UnityConfig.GetConfiguredContainer().Resolve<IUltilityService>();
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
                throw new AuthenticationException("User must login to access this private api");
            } 

            // Neu token het han thi khong cho goi may ham private ... TODO

            return Task.FromResult(0);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}