using System.Net;
using Newtonsoft.Json;
using Shopping.Contexts.Auth.Applications.DTOs;
using Shopping.Contexts.Auth.Applications.Interfaces;
using System;
using Shopping.App_Start;
using Shopping.Models;

namespace Shopping.Contexts.Auth.Services
{
    public class AuthService : IAuthService
    {
        public static readonly string Url = "https://graph.facebook.com/me";
        public static readonly string Fields = "id,name";

        public UserDto GetUserInfoFromToken(string token)
        {
            using (var client = new WebClient())
            {
                string userJson = null;

                try
                {
                    userJson = client.DownloadString(Url + "?access_token=" + token + "&fields=" + Fields);

                } catch(Exception e)
                {
                    throw new BadRequestException("Invalid token");
                }

                dynamic userDynamic = JsonConvert.DeserializeObject(userJson);


                User user = new User();
                
                user.FbId = userDynamic["id"];
                user.Name = userDynamic["name"];

                return new UserDto(user);
            }

        }

        public string NormalizeUri()
        {
            throw new NotImplementedException();
        }

        public string UnNormalizeUri()
        {
            throw new NotImplementedException();
        }
    }
}