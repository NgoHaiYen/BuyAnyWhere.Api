using Shopping.Contexts.Auth.Applications.Interfaces;
using Shopping.Models;
using System.Web.Http;

namespace Shopping.Contexts.Auth.Applications.Controllers
{

    [RoutePrefix("api/Auth/Roles")]
    public class RoleController : ApiController
    {
        private readonly IRoleService roleService;

        public RoleController(IRoleService roleService)
        {
            this.roleService = roleService;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(roleService.Get());
        }
    }
}
