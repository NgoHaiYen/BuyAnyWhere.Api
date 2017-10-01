using Shopping.Contexts.Auth.Applications.DTOs;
using Shopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Shopping.Contexts.Auth.Applications.Controllers
{

    [RoutePrefix("api/Notification")]
    public class NotificationController : ApiController
    {
        private readonly ShoppingEntities shoppingEntities;

        public NotificationController(ShoppingEntities shoppingEntities)
        {
            this.shoppingEntities = shoppingEntities;
        }

        [HttpPut]
        [Route("{userId}")]
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
