using Shopping.Contexts.Auths.Domains.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopping.Applications.DTOs
{
    public class UserDTO
    {
        public UserDTO() { }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public UserDTO(User user)
        {
            Id = user.Id;
            Name = user.Name;
        }

        public User ToModel()
        {
            Id = Guid.NewGuid();
            User user = new User();
            user.Id = Id;
            user.Name = Name;

            return user;
        }

        public void Update(User user)
        {
            Id = user.Id;
            Name = user.Name;
        }

    }
}