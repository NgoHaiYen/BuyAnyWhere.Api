using Shopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopping.Contexts.Auth.Applications.DTOs
{
    public class ApiDto
    {
        public Guid Id { get; set; }
        public string Method { get; set; }
        public string Uri { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }


        public ApiDto()
        {

        }

        public ApiDto(Api api)
        {
            this.Id = api.Id;
            this.Method = api.Method;
            this.Uri = api.Uri;
            this.Name = api.Name;
            this.Description = api.Description;
            this.Type = api.Type;
        }

        public Api ToModel()
        {
            return new Api()
            {
                Id = Id,
                Method = Method,
                Uri = Uri,
                Name = Name,
                Description = Description,
                Type = Type,
            };
        }
    }
}