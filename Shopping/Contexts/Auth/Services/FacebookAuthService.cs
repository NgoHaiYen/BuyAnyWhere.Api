﻿using System.Net;
using Newtonsoft.Json;
using Shopping.Contexts.Auth.Applications.DTOs;
using Shopping.Contexts.Auth.Applications.Interfaces;
using System;
using Shopping.App_Start;
using Shopping.Models;

namespace Shopping.Contexts.Auth.Services
{
    public class FacebookAuthService : IAuthService
    {
        public static readonly string Url = "https://graph.facebook.com/me";
        public static readonly string Fields = "id,name,email";

        private readonly ShoppingEntities shoppingEntities;

        public FacebookAuthService(ShoppingEntities shoppingEntities)
        {
            this.shoppingEntities = shoppingEntities;
        }

        public UserDto GetUserFromProviderToken(string token)
        {
            using (var client = new WebClient())
            {

                try
                {
                    string userJson = client.DownloadString(Url + "?access_token=" + token + "&fields=" + Fields);

                    dynamic userDynamic = JsonConvert.DeserializeObject(userJson);

                    UserDto userDto = new UserDto
                    {
                        FacebookId = userDynamic["id"],
                        Name = userDynamic["name"],
                        Email = userDynamic["email"]
                    };
                    return userDto;

                } catch(Exception)
                {
                    throw new BadRequestException("Invalid token");
                }
            }
        }
    }
}