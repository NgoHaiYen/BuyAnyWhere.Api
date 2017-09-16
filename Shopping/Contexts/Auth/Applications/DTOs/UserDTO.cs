using System;
using Shopping.Models;

namespace Shopping.Contexts.Auth.Applications.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FbId { get; set; }
        public string Name { get; set; }


        public UserDto() { }


        public UserDto(User user)
        {
            Id = user.Id;          
            Name = user.Name;
            FbId = user.FbId;
        }

        public User ToModel()
        {
            User user = new User();

            user.Id = Guid.NewGuid();
            user.Name = Name;
            user.FbId = FbId;

            return user;
        }

        public void Update(User user)
        {
            Id = user.Id;
            Name = user.Name;
            FbId = user.FbId;
        }

    }
}