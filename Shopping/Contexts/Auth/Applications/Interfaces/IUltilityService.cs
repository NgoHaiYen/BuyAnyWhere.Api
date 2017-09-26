using Shopping.Models;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Shopping.Contexts.Auth.Applications.Interfaces
{
    public interface IUltilityService
    {
        string GetHeaderToken(HttpAuthenticationContext context);
        string GetHeaderToken(HttpActionExecutedContext context);
        string GetHeaderToken(HttpContext context);
        string GetHeaderToken(HttpActionContext context);

        string NormalizePath(string path);
        User GetUserForLogging(string token);
        User GetUserFromToken(string token);
        void Log(object context, string headerToken, bool success, string reason);
    }
}
