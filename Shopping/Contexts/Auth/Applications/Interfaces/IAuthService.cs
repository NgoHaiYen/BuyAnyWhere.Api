﻿using Shopping.Contexts.Auth.Applications.DTOs;

namespace Shopping.Contexts.Auth.Applications.Interfaces
{
    public interface IAuthService
    {
        UserDto GetUserFromTokenProvider(string token);
    }
}
