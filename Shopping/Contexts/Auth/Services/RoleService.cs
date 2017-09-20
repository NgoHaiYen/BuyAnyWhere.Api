using Shopping.Contexts.Auth.Applications.DTOs;
using Shopping.Contexts.Auth.Applications.Interfaces;
using Shopping.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Shopping.Contexts.Auth.Services
{
    public class RoleService : IRoleService
    {
        private readonly ShoppingEntities shoppingEntities;

        public RoleService(ShoppingEntities shoppingEntities)
        {
            this.shoppingEntities = shoppingEntities;
        }

        public List<RoleDto> Get()
        {
            var roles = shoppingEntities.Roles.Include(t => t.Users).ToList();

            var roleDtos = roles.ConvertAll(t => new RoleDto(t, t.Users));

            return roleDtos;
        }
    }
}