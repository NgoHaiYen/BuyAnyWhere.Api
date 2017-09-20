using System.Web.Http;
using Shopping.Contexts.Auth.Applications.DTOs;
using Shopping.Contexts.Auth.Applications.Interfaces;
using Shopping.Models;
using System.Linq;
using Shopping.Applications.Interfaces;
using System;
using Shopping.Contexts.Auth.Services;

namespace Shopping.Contexts.Auth.Applications.Controllers
{
    [RoutePrefix("api/Auth/OAuth2")]
    public class AuthController : ApiController
    {

        private readonly IUserService userService;
        private readonly ShoppingEntities shoppingEntities;

        public AuthController(ShoppingEntities shoppingEntities,
            IUserService userService)
        {
            this.shoppingEntities = shoppingEntities;
            this.userService = userService;
        }


        [HttpPost]
        [Route("Facebook/Callback")]
        public IHttpActionResult Login([FromBody] string token)
        {
            IAuthService authService = new FacebookAuthService(shoppingEntities);

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
