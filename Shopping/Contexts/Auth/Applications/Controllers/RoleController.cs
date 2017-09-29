using System.Linq;
using System.Web.Http;
using Shopping.Models;
using Shopping.Contexts.Auth.Applications.DTOs;
using System;
using System.Data.Entity;

namespace Shopping.Contexts.Auth.Applications.Controllers
{
    [RoutePrefix("api/Auth/Roles")]
    public class RoleController : ApiController
    {
        private readonly ShoppingEntities shoppingEntities;

        public RoleController(ShoppingEntities shoppingEntities)
        {
            this.shoppingEntities = shoppingEntities;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get([FromUri] RoleFilterDto roleFilterDto)
        {
            if (roleFilterDto == null)
            {
                roleFilterDto = new RoleFilterDto();
            }

            var roles = roleFilterDto.SkipAndTake(roleFilterDto.ApplyTo(
                shoppingEntities.Roles)).ToList();

            var roleDtos = roles.Select(t => new RoleDto(t)).ToList();

            return Ok(roleDtos);
        }

        [HttpGet]
        [Route("{roleId}")]
        public IHttpActionResult Get([FromUri] Guid roleId)
        {
            var role = shoppingEntities.Roles.FirstOrDefault(t => t.Id == roleId);

            if (role == null)
            {
                throw new BadRequestException("Role không tồn tại");
            }

            var roleDto = new RoleDto(role);

            return Ok(roleDto);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody] RoleDto roleDto)
        {
            Role role = roleDto.ToModel();
            shoppingEntities.Roles.Add(role);
            shoppingEntities.SaveChanges();

            return Ok(new RoleDto(role));
        }

        [HttpPut]
        [Route("{roleId}")]
        public IHttpActionResult Put(Guid roleId, RoleDto roleDto)
        {
            var role = shoppingEntities.Roles.FirstOrDefault(t => t.Id == roleId);

            if (role == null)
            {
                throw new BadRequestException("Role không tồn tại");
            }

            roleDto.ToModel(role);
     
            shoppingEntities.SaveChanges();
            return Ok(new RoleDto(role));
        }
    }
}