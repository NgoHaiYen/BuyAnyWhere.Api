using Shopping.Models;

namespace Shopping.Contexts.Auth.Applications.Interfaces
{
    public interface IUltilityService
    {
        string GetTokenFromHeaderHttpRequest(object context);
        string NormalizePath(string path);
        User GetUserFromTokenAlwayReturnUserName(string token);
    }
}
