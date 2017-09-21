using Shopping.Applications.Interfaces;
using Shopping.Contexts.Auth.Applications.DTOs;
using Shopping.Contexts.Auth.Applications.Interfaces;
using Shopping.Ultilities;
using System;
using System.Web.Http;

namespace Shopping.Contexts.Auth.Applications.Controllers
{

    [RoutePrefix("api/Auth/Users")]
    public class UserController : ApiController
    {

        private readonly IUserService userService;
        private readonly IUltilityService ultilityService;

        public UserController(IUserService userService, IUltilityService ultilityService)
        {
            this.userService = userService;
            this.ultilityService = ultilityService;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get(PaginateDto paginateDto)
        {
            return Ok(userService.Get(paginateDto));
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get([FromUri] Guid id)
        {
            return Ok(userService.Get(id));
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] UserDto userDto)
        {
            return Ok(userDto.ToModel());
        }

    }
}
