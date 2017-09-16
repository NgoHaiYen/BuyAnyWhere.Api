using System.Web.Http;
using Shopping.Contexts.Auth.Applications.DTOs;
using Shopping.Contexts.Auth.Applications.Interfaces;
using Shopping.Ultilities;
using Shopping.Models;
using System.Linq;
using Shopping.Applications.Interfaces;

namespace Shopping.Contexts.Auth.Applications.Controllers
{
    [RoutePrefix("api/Auths")]
    public class AuthController : CommonController
    {

        private readonly IAuthService authService;
        private readonly ShoppingEntities shoppingEntities;

        public AuthController(IAuthService authService, ShoppingEntities shoppingEntities)
        {
            this.authService = authService;
            this.shoppingEntities = shoppingEntities;
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public IHttpActionResult Login([FromBody] string token)
        {

            UserDto userDto = authService.GetUserInfoFromToken(token);

            User user = shoppingEntities.Users.FirstOrDefault(t => t.FbId == userDto.FbId);

            if (user == null)
            {
                // Chua co user, them thong tin user vao database
                shoppingEntities.Users.Add(userDto.ToModel());

                // Tao tao token cho user
                UserTokenDTO userTokenDto = new UserTokenDTO();
                userTokenDto.Name = token;
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

            return Return("Login completed !");
            
        }
    }
}
