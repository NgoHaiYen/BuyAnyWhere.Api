﻿using System;
using System.Collections.Generic;
using System.Linq;
using Shopping.Applications.Interfaces;
using Shopping.Contexts.Auths.Applications.DTOs;
using Shopping.Contexts.Auths.Infrastructures.Models;

namespace Shopping.Contexts.Auths.Domains.Services
{
    public class UserService : IUserService
    {
        public UserDto Create(UserDto userDto)
        {
            using (ShoppingEntities shoppingEntities = new ShoppingEntities())
            {
                User user = userDto.ToModel();
                shoppingEntities.Users.Add(userDto.ToModel());

                return Get(user.Id);
            }
        }

        public List<UserDto> Get()
        {
            using (ShoppingEntities shoppingEntities = new ShoppingEntities())
            {
                var users = shoppingEntities.Users.ToList();

                return users.ConvertAll(t => new UserDto(t));
            }
        }

        public UserDto Get(Guid id)
        {
            using (ShoppingEntities shoppingEntities = new ShoppingEntities())
            {
                var user = shoppingEntities.Users.FirstOrDefault(t => t.Id == id);

                if (user == null)
                {
                    throw new Exception("User not found!");
                }

                return new UserDto(user);
            }
        }
    }
}