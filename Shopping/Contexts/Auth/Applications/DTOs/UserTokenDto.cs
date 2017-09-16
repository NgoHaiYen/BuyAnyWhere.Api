using Shopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopping.Contexts.Auth.Applications.DTOs
{
    public class UserTokenDTO
    {
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? UserId { get; set; }


        public UserTokenDTO()
        {

        }

        public UserTokenDTO(UserToken userToken)
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