﻿using System.Web.Http;
using Shopping.Contexts.Auth.Applications.DTOs;
using Shopping.Contexts.Auth.Applications.Interfaces;
using Shopping.Ultilities;
using Shopping.Models;
using System.Linq;
using Shopping.Applications.Interfaces;
using System;

namespace Shopping.Contexts.Auth.Applications.Controllers
{
    [RoutePrefix("api/OAuth2")]
    public class AuthController : ApiController
    {

        private readonly IAuthService authService;
        private readonly IUserService userService;
        private readonly ShoppingEntities shoppingEntities;

        public AuthController(ShoppingEntities shoppingEntities,
            IUserService userService)
        {
            this.shoppingEntities = shoppingEntities;
            this.userService = userService;
        }


        [HttpPost]
        [Route("Facebook/Login")]
        public IHttpActionResult Login([FromBody] string token)
        {

            UserDto userDto = authService.GetUserInfoFromToken(token);

            User user = shoppingEntities.Users.FirstOrDefault(t => t.FbId == userDto.FbId);

            if (user == null)
            {
                // Chua co user, them thong tin user vao database
                user = userDto.ToModel();
                shoppingEntities.Users.Add(user);

                // Tao tao token cho user
                UserToken userToken = new UserToken();
                userToken.Id = Guid.NewGuid();
                userToken.Name = token;
                userToken.UserId = user.Id;

                shoppingEntities.UserTokens.Add(userToken);

            } else
            {
                // Kiem tra neu token khac thi them vao
                UserToken userToken = shoppingEntities.UserTokens.FirstOrDefault(t => t.Name == token);

                if (userToken == null)
                {
                    userToken = new UserToken();
                    userToken.Id = Guid.NewGuid();
                    userToken.Name = token;
                    userToken.UserId = user.Id;

                    shoppingEntities.UserTokens.Add(userToken);
                }
            }

            shoppingEntities.SaveChanges();

            return Ok(userService.Get(user.Id));
            
        }
    }
}
