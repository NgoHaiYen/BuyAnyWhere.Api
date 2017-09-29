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


        public RoleDto() { }

        public RoleDto(Role role)
        {
            this.Id = role.Id;
            this.Name = role.Name;
            this.Description = role.Description;

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