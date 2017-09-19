﻿using System.Net;
using Newtonsoft.Json;
using Shopping.Contexts.Auth.Applications.DTOs;
using Shopping.Contexts.Auth.Applications.Interfaces;
using System;
using Shopping.App_Start;
using Shopping.Models;
using System.Linq;
using System.Collections.Generic;
using Shopping.Ultilities;

namespace Shopping.Contexts.Auth.Services
{
    public class AuthService : IAuthService
    {
        public static readonly string Url = "https://graph.facebook.com/me";
        public static readonly string Fields = "id,name";

        private readonly ShoppingEntities shoppingEntities;

        public AuthService(ShoppingEntities shoppingEntities)
        {
            this.shoppingEntities = shoppingEntities;
        }


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


                UserDto userDto = new UserDto();

                userDto.FbId = userDynamic["id"];
                userDto.Name = userDynamic["name"];

                return userDto;
            }

        }

    }
}