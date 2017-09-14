using Shopping.Applications.DTOs;
using Shopping.Applications.Interfaces;
using Shopping.Domains.Services;
using Shopping.Infrastructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Shopping.Applications.Controllers
{
    [RoutePrefix("api/Users")]
    public class UserController : CommonController
    {
        private IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
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
        public IHttpActionResult Post([FromBody] UserDTO userDTO)
        {
            return Return(userDTO.ToModel());
        }
    }
}
