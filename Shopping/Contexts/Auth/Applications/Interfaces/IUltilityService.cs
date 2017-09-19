using Shopping.Models;
using System.Web.Http.Filters;

namespace Shopping.Contexts.Auth.Applications.Interfaces
{
    public interface IUltilityService
    {
        string GetTokenFromHeaderHttpRequest(HttpAuthenticationContext context);
        string GetTokenFromHeaderHttpRequest(HttpActionExecutedContext context);

        string NormalizePath(string path);
        User GetUserFromTokenAlwayReturnUserName(string token);
        void Log(object context, string headerToken, bool success, string reason);
    }
}
