using Shopping.Contexts.Auth.Applications.DTOs;
using Shopping.Models;
using Shopping.Ultilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Shopping.Contexts.Auth.Applications.Controllers
{
    public class NotificationController : ApiController
    {
        private readonly ShoppingEntities shoppingEntities;
        private readonly UltilityService ultilityService;

        public NotificationController(ShoppingEntities shoppingEntities, UltilityService ultilityService)
        {
            this.shoppingEntities = shoppingEntities;
            this.ultilityService = ultilityService;
        }

        [HttpPut]
        [Route("api/Auth/Users/current/Notification")]
        public IHttpActionResult PutNotification([FromBody] bool state)
        {

            var token = ultilityService.GetHeaderToken(HttpContext.Current);

            var userToken = shoppingEntities.UserTokens.FirstOrDefault(t => t.Name == token);

            if (userToken == null)
            {
                throw new BadRequestException("Access token khong hop le");
            }

            var user = userToken.User;

            if (user == null)
            {
                throw new BadRequestException("Không tồn tại User");
            }

            user.Notification = state;
            shoppingEntities.SaveChanges();

            return Ok(new UserDto(user));
        }

        [HttpPut]
        [Route("api/Auth/Users/{userId}/Notification")]
        public IHttpActionResult PutNotification([FromUri] Guid userId, [FromBody] bool state)
        {
            var user = shoppingEntities.Users.FirstOrDefault(t => t.Id == userId);
            if (user == null)
            {
                throw new BadRequestException("Không tồn tại User");
            }

            user.Notification = state;
            shoppingEntities.SaveChanges();

            return Ok(new UserDto(user));
        }       
    }
}
