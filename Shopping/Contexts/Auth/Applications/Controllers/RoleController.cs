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


        [HttpGet]
        [Route("{roleId}/Apis")]
        public IHttpActionResult GetRoleApis([FromUri] Guid roleId, [FromUri] ApiFilterDto apiFilterDto)
        {
            if (apiFilterDto == null)
            {
                apiFilterDto = new ApiFilterDto();
            }

            var role = shoppingEntities.Roles.Include(t => t.Apis).FirstOrDefault(t => t.Id == roleId);
            if (role == null)
            {
                throw new BadRequestException("Khong ton tai role");
            }

            var apis = apiFilterDto.SkipAndTake(apiFilterDto.ApplyTo(role.Apis.AsQueryable())).ToList();

            var apiDtos = apis.ConvertAll(t => new ApiDto(t));

            return Ok(apiDtos);
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

        [HttpGet]
        [Route("{roleId}/Users")]
        public IHttpActionResult GetRoleUsers([FromUri] Guid roleId, [FromUri] UserFilterDto userFilterDto)
        {
            if (userFilterDto == null)
            {
                userFilterDto = new UserFilterDto();
            }

            var role = shoppingEntities.Roles.Include(t => t.Users).FirstOrDefault(t => t.Id == roleId);

            if (role == null)
            {
                throw new BadRequestException("Role khong ton tai");
            }

            var users = userFilterDto.SkipAndTake(userFilterDto.ApplyTo(role.Users.AsQueryable())).ToList();

            var userDtos = users.ConvertAll(t => new UserDto(t));

            return Ok(userDtos);
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

        [HttpPut]
        [Route("{roleId}/Apis/{apiId}")]
        public IHttpActionResult PutApi([FromUri] Guid roleId, [FromUri] Guid apiId)
        {
            var role = shoppingEntities.Roles.FirstOrDefault(t => t.Id == roleId);
            var api = shoppingEntities.Apis.FirstOrDefault(t => t.Id == apiId);
            if (role == null)
            {
                throw new BadRequestException("Khong ton tai Role");
            }
            if (api == null)
            {
                throw new BadRequestException("Khong ton tai Api");
            }

            if (!role.Apis.Contains(api))
            {
                role.Apis.Add(api);
            }

            shoppingEntities.SaveChanges();

            return Ok(new RoleDto(role));
        }
    }
}