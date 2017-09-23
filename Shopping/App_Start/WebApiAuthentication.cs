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
            var publicApis = shoppingEntities.Apis.Where(t => t.Type == (int)Constant.TypeApi.Public).ToList();

            // Nếu là PUBLIC API 

            if (publicApis.FirstOrDefault(t => t.Method == request.Method.ToString() 
                && t.Uri == appService.NormalizePath(request.RequestUri.AbsolutePath)) != null)
            {
                return Task.FromResult(0);
            }


            // Nếu là PRIVATE API 

            string token = appService.GetHeaderToken(context);
            var userToken = shoppingEntities.UserTokens.FirstOrDefault(t => t.Name == token);

            if (userToken == null)
            {
                throw new AuthenticationException("Đường dẫn này yêu cầu người dùng đăng nhập");
            } 

            // Nếu token hết hạn thì 

            return Task.FromResult(0);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}