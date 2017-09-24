using System;
using Shopping.Models;

namespace Shopping.Contexts.Auth.Applications.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FacebookId { get; set; }
        public string GoogleId { get; set; }
        public string Name { get; set; }

        public string AccessToken { get; set; }
        public RoleDto RoleDto { get; set; }


        public UserDto() { }


        public UserDto(User user, string accessToken = null, 
            params object[] args)
        {
            Id = user.Id;          
            Name = user.Name;
            FacebookId = user.FacebookId;
            AccessToken = accessToken;
            GoogleId = user.GoogleId;
            
            foreach (var arg in args)
            {
                if (arg is Role role)
                {                 
                    RoleDto = new RoleDto(role);
                }
            }
        }

        public User ToModel()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = Name,
                FacebookId = FacebookId,
                GoogleId = GoogleId
            };


            return user;
        }

    }
}