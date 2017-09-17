using System.Web.Http;
using Shopping.Contexts.Auth.Applications.DTOs;
using Shopping.Contexts.Auth.Applications.Interfaces;
using Shopping.Ultilities;
using Shopping.Models;
using System.Linq;
using Shopping.Applications.Interfaces;

namespace Shopping.Contexts.Auth.Applications.Controllers
{
    [RoutePrefix("api/Auth")]
    public class AuthController : CommonController
    {

        private readonly IAuthService authService;
        private readonly IUserService userService;
        private readonly ShoppingEntities shoppingEntities;

        public AuthController(IAuthService authService, ShoppingEntities shoppingEntities,
            IUserService userService)
        {
            this.authService = authService;
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
                UserTokenDTO userTokenDto = new UserTokenDTO();
                userTokenDto.Name = token;
                userTokenDto.UserId = user.Id;

                shoppingEntities.UserTokens.Add(userTokenDto.ToModel());

            } else
            {
                // Da ton tai user roi, update thong tin user
                user.Name = userDto.Name;

                // Kiem tra neu token khac thi them vao
                UserToken userToken = shoppingEntities.UserTokens.FirstOrDefault(t => t.Name == token);
                if (userToken == null)
                {
                    UserTokenDTO userTokenDto = new UserTokenDTO();
                    userTokenDto.Name = token;
                    userTokenDto.UserId = user.Id;

                    shoppingEntities.UserTokens.Add(userTokenDto.ToModel());
                }
            }

            shoppingEntities.SaveChanges();

            return Return(userService.Get(user.Id));
            
        }
    }
}
