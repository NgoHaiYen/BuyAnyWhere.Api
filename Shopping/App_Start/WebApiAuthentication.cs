using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;
using Microsoft.Practices.Unity;
using Shopping.Applications.Interfaces;

namespace Shopping.App_Start
{
    public class WebApiAuthentication : IAuthenticationFilter
    {
        public static readonly string AccessToken = "access_token";
        private readonly IUserService userService;


        public WebApiAuthentication()
        {
            userService = UnityConfig.GetConfiguredContainer().Resolve<IUserService>();
        }


        public bool AllowMultiple => true;

        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            string token;

            if (context.Request.Headers.TryGetValues(AccessToken, out var values))
            {
                token = values.First();
            }

      
            return Task.CompletedTask;
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}