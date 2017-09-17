using Shopping.Contexts.Auth.Applications.DTOs;
using Shopping.Models;
using System.Collections.Generic;

namespace Shopping.Contexts.Auth.Applications.Interfaces
{
    public interface IAuthService
    {
        UserDto GetUserInfoFromToken(string token);

        string NormalizePath(string path);

    }
}
