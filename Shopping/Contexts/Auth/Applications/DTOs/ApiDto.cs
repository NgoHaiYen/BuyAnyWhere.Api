using Shopping.Models;
using Shopping.Ultilities;
using System;
using System.Collections.Generic;

namespace Shopping.Contexts.Auth.Applications.DTOs
{
    public class ApiDto
    {
        public Guid Id { get; set; }
        public string Method { get; set; }
        public string Uri { get; set; }
        public string Name { get; set; }
        public Constant.ApiType Type { get; set; }



        public ApiDto()
        {

        }

        public ApiDto(Api api)
        {
            this.Id = api.Id;
            this.Method = api.Method;
            this.Uri = api.Uri;
            if (api.Type != null) this.Type = (Constant.ApiType) api.Type;
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