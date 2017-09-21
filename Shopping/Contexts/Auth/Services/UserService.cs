using System;
using System.Collections.Generic;
using System.Linq;
using Shopping.Applications.Interfaces;
using Shopping.Contexts.Auth.Applications.DTOs;
using Shopping.Models;
using Shopping.Ultilities;
using Shopping.App_Start;

namespace Shopping.Contexts.Auth.Services
{
    public class UserService : IUserService
    {
        private readonly ShoppingEntities shoppingEntities;

        public UserService(ShoppingEntities shoppingEntities)
        {
            this.shoppingEntities = shoppingEntities;
        }

        public UserDto Create(UserDto userDto)
        {
            User user = userDto.ToModel();
            shoppingEntities.Users.Add(userDto.ToModel());

            return Get(user.Id);   
        }

        public List<UserDto> Get(PaginateDto paginateDto)
        {
            if (paginateDto == null)
            {
                paginateDto = new PaginateDto();
            }

            var users = paginateDto.SkipAndTake(shoppingEntities.Users).ToList();

            return users.ConvertAll(t => new UserDto(t, null, t.Role));     
        }

        public UserDto Get(Guid userId)
        {      
            var user = shoppingEntities.Users.FirstOrDefault(t => t.Id == userId);

            if (user == null)
            {
                throw new Exception("User not found!");
            }

            return new UserDto(user, null, user.Role);       
        }

        public UserDto PutRole(Guid userId, Guid roleId)
        {
            User user = shoppingEntities.Users.FirstOrDefault(t => t.Id == userId);
            Role role = shoppingEntities.Roles.FirstOrDefault(t => t.Id == roleId);

            if (user == null)
            {
                throw new BadRequestException("ID người dùng không hợp lệ");
            }

            if (role == null)
            {
                throw new BadRequestException("ID vai trò không hợp lệ");
            }


            user.Role = role;

            shoppingEntities.SaveChanges();

            return Get(userId);
        }
    }
}