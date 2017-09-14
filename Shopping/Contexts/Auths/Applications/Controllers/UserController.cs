using System;
using System.Net.Http;
using System.Web.Http;
using Shopping.Applications.Interfaces;
using Shopping.Contexts.Auths.Applications.DTOs;
using Shopping.Contexts.Auths.Infrastructures;

namespace Shopping.Contexts.Auths.Applications.Controllers
{
    [RoutePrefix("api/Users")]
    public class UserController : CommonController
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            HttpResponseMessage httpResponseMessage = Request.CreateResponse();
            httpResponseMessage.Headers.Location = new Uri("");
            return Return(userService.Get());
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get([FromUri] Guid id)
        {
            return Return(userService.Get(id));
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] UserDto userDto)
        {
            return Return(userDto.ToModel());
        }
    }
}
