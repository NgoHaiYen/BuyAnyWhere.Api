using System;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using Shopping.Contexts.Auth.Applications.DTOs;
using Shopping.Contexts.Auth.Applications.Interfaces;
using Shopping.Contexts.Auth.Services;
using Shopping.Models;

namespace Shopping.Contexts.Auth.Applications.Controllers
{
    [RoutePrefix("api/Auth/OAuth2")]
    public class AuthController : ApiController
    {
        private readonly ShoppingEntities shoppingEntities;

        public AuthController(ShoppingEntities shoppingEntities)
        {
            this.shoppingEntities = shoppingEntities;
        }

        [HttpPost]
        [Route("Facebook/Callback")]
        public IHttpActionResult Login([FromBody] string token)
        {
            IAuthService authService = new FacebookAuthService(shoppingEntities);
            var userDto = authService.GetUserFromProviderToken(token);
            var user = shoppingEntities.Users.FirstOrDefault(t => t.FacebookId == userDto.FacebookId);

            if (user == null)
            {
                user = userDto.ToModel();
                shoppingEntities.Users.Add(user);

                var userToken = new UserToken
                {
                    Id = Guid.NewGuid(),
                    Name = token,
                    UserId = user.Id
                };

                shoppingEntities.UserTokens.Add(userToken);
            }
            else
            {
                var userToken = shoppingEntities.UserTokens.FirstOrDefault(t => t.Name == token);

                if (userToken == null)
                {
                    userToken = new UserToken
                    {
                        Id = Guid.NewGuid(),
                        Name = token,
                        UserId = user.Id
                    };
                    shoppingEntities.UserTokens.Add(userToken);
                }
            }

            shoppingEntities.SaveChanges();

            var usr = shoppingEntities.Users.FirstOrDefault(t => t.Id == user.Id);
            var usrDto = new UserDto(usr);
            userDto.AccessToken = token;
            return Ok(usrDto);
        }
    }
}