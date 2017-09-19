using Shopping.Contexts.Auth.Applications.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shopping.Contexts.Auth.Applications.DTOs;

namespace Shopping.Contexts.Auth.Services
{
    public class GoogleAuthService : IAuthService
    {
        public UserDto GetUserInfoFromToken(string token)
        {
            throw new NotImplementedException();
        }
    }
}