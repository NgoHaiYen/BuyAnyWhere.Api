using System;
using System.Web.Http;
using Shopping.Applications.Interfaces;
using Shopping.Contexts.Auth.Applications.DTOs;
using Shopping.Ultilities;

namespace Shopping.Contexts.Auth.Applications.Controllers
{
    public class UserController : CommonController
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        [Route("api/Users")]
        public IHttpActionResult Get()
        {
            return Return(userService.Get());
        }

        [HttpGet]
        [Route("api/Users/{id}")]
        public IHttpActionResult Get([FromUri] Guid id)
        {
            return Return(userService.Get(id));
        }

        [HttpPost]
        [Route("api/Users")]
        public IHttpActionResult Post([FromBody] UserDto userDto)
        {
            return Return(userDto.ToModel());
        }
    }
}
