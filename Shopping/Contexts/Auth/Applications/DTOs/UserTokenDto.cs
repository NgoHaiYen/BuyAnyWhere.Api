using Shopping.Models;
using System;

namespace Shopping.Contexts.Auth.Applications.DTOs
{
    public class UserTokenDto
    {
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? UserId { get; set; }

        public UserTokenDto()
        {

        }

        public UserTokenDto(UserToken userToken)
        {
            Id = userToken.Id;
            Name = userToken.Name;
            UserId = userToken.UserId;
        }

        public UserToken ToModel()
        {
            UserToken userToken = new UserToken();
            userToken.Id = Guid.NewGuid();
            userToken.Name = Name;
            userToken.UserId = UserId;

            return userToken;
        }
    }
}