using Shopping.Models;
using System;
using System.Collections.Generic;

namespace Shopping.Contexts.Auth.Applications.DTOs
{
    public class RoleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<UserDto> UserDtos { get; set; }
        public List<ApiDto> ApiDtos { get; set; }


        public RoleDto() { }

        public RoleDto(Role role, params object[] args)
        {
            this.Id = role.Id;
            this.Name = role.Name;
            this.Description = role.Description;

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

                if (arg is ICollection<Api> apis)
                {
                    ApiDtos = new List<ApiDto>();

                    foreach (var api in apis)
                    {
                        ApiDtos.Add(new ApiDto(api));
                    }
                }

            }
        }

        public Role ToModel(Role role = null)
        {
            if (role == null)
            {
                role = new Role();
                role.Id = Guid.NewGuid();
            }

            role.Name = Name;
            role.Description = Description;

            return role;          
        }
    }
} 