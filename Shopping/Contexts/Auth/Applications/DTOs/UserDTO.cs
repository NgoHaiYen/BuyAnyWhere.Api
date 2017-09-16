using System;
using Shopping.Models;

namespace Shopping.Contexts.Auths.Applications.DTOs
{
    public class UserDto
    {
        public UserDto() { }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public UserDto(User user)
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