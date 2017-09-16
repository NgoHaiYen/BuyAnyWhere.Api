using Shopping.Contexts.Auth.Applications.DTOs;
using Shopping.Contexts.Auths.Applications.DTOs;

namespace Shopping.Contexts.Auth.Applications.Interfaces
{
    public interface IAuthService
    {
        UserDto GetUserInfoFromToken(string token);
    }
}
