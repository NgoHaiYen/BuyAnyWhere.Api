using Shopping.Models;
using Shopping.Ultilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopping.Contexts.Auth.Applications.DTOs
{
    public class ApiFilterDto : FilterDto<Api>
    {
        public string Method { get; set; }
        public string Uri { get; set; }
        public Constant.ApiType? Type { get; set; }

        public override IQueryable<Api> ApplyTo(IQueryable<Api> source)
        {
            if (! string.IsNullOrEmpty(Method))
            {
                source = source.Where(t => t.Method == Method);
            }

            if (!string.IsNullOrEmpty(Method))
            {
                source = source.Where(t => t.Uri == Uri);
            }

            if (Type.HasValue)
            {
                source = source.Where(t => t.Type == (int)Type);
            }

            return Order(source);
        }
    }
}