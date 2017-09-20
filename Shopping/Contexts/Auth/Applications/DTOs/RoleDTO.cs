﻿using Shopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopping.Contexts.Auth.Applications.DTOs
{
    public class RoleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public List<UserDto> UserDtos { get; set; }


        public RoleDto()
        {

        }

        public RoleDto(Role role, params object[] args)
        {
            this.Id = role.Id;
            this.Name = role.Name;

            foreach(var arg in args)
            {
                if (arg is ICollection<User> users)
                {
                    UserDtos = new List<UserDto>();

                    foreach(var user in users)
                    {
                        UserDtos.Add(new UserDto(user));
                    }
                }
            }
        }

        public Role ToModel()
        {
            Role role = new Role();

            role.Id = Guid.NewGuid();
            role.Name = Name;
            return role;
        }

    }
} 