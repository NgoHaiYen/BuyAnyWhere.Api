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
        public string CloudToken { get; set; }

        public string AvatarUrl { get; set; }
        public string Email { get; set; }
        
        public string Gender { get; set; }

        public UserDto() { }


        public UserDto(User user)
        {
            Id = user.Id;          
            Name = user.Name;
            FacebookId = user.FacebookId;
            CloudToken = user.CloudToken;
            GoogleId = user.GoogleId;
            AvatarUrl = user.AvatarUrl;
            Email = user.Email;
            Gender = user.Gender;
        }

        public User ToModel()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = Name,
                FacebookId = FacebookId,
                GoogleId = GoogleId,
                CloudToken = CloudToken,
                AvatarUrl = AvatarUrl,
                Email = Email,
                Gender = Gender
            };


            return user;
        }

    }
}