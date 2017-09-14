using Shopping.Applications.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shopping.Applications.DTOs;
using Shopping.Contexts.Auths.Domains.Models;

namespace Shopping.Domains.Services
{
    public class UserService : IUserService
    {
        public UserDTO Create(UserDTO userDTO)
        {
            using (ShoppingEntities shoppingEntities = new ShoppingEntities())
            {
                User user = userDTO.ToModel();
                shoppingEntities.Users.Add(userDTO.ToModel());

                return Get(user.Id);
            }
        }

        public List<UserDTO> Get()
        {
            using (ShoppingEntities shoppingEntities = new ShoppingEntities())
            {
                var users = shoppingEntities.Users.ToList();

                return users.ConvertAll(t => new UserDTO(t));
            }
        }

        public UserDTO Get(Guid id)
        {
            using (ShoppingEntities shoppingEntities = new ShoppingEntities())
            {
                var user = shoppingEntities.Users.FirstOrDefault(t => t.Id == id);

                if (user == null)
                {
                    throw new Exception("User not found!");
                }

                return new UserDTO(user);
            }
        }
    }
}