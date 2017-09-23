using Shopping.Models;
using Shopping.Ultilities;
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
        public Constant.TypeApi Type { get; set; }


        public ApiDto()
        {

        }

        public ApiDto(Api api)
        {
            this.Id = api.Id;
            this.Method = api.Method;
            this.Uri = api.Uri;
            if (api.Type != null) this.Type = (Constant.TypeApi) api.Type;
        }

        public Api ToModel()
        {
            return new Api()
            {
                Id = Id,
                Method = Method,
                Uri = Uri,
                Type = (int)Type,
            };
        }
    }
}