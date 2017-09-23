using System;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Shopping.App_Start;
using Shopping.Contexts.Auth.Applications.Interfaces;
using Shopping.Models;

namespace Shopping
{
    public class ExceptionResponeAttribute : ExceptionFilterAttribute
    {
        private readonly ShoppingEntities shoppingEntities;
        private readonly IUltilityService ultilityService;

        public ExceptionResponeAttribute()
        {
            this.shoppingEntities = UnityConfig.GetConfiguredContainer().Resolve<ShoppingEntities>();
            this.ultilityService = UnityConfig.GetConfiguredContainer().Resolve<IUltilityService>();
        }

        public override void OnException(HttpActionExecutedContext context)
        {
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            if (context.Exception is BadRequestException)
            {
                response = new HttpResponseMessage(HttpStatusCode.BadRequest) {ReasonPhrase = "Wrong paramenters"};

            }
            if (context.Exception is ConflictException)
            {
                response = new HttpResponseMessage(HttpStatusCode.Conflict) {ReasonPhrase = "Conflict data"};
            }

            if (context.Exception is ForbiddenException)
            {
                response = new HttpResponseMessage(HttpStatusCode.Forbidden) {ReasonPhrase = "Forbidden"};
            }
            if (context.Exception is NotFoundException)
            {
                response = new HttpResponseMessage(HttpStatusCode.NotFound) {ReasonPhrase = "Not Found"};
            }

            if (context.Exception is DataException)
            {
                response = new HttpResponseMessage(HttpStatusCode.NotModified) {ReasonPhrase = "Not Modified Data"};
            }

            var message = JsonConvert.SerializeObject(new { Message = context.Exception.Message });

            response.Content = new StringContent(message);
            context.Response = response;


            var token = ultilityService.GetHeaderToken(context);
            ultilityService.Log(context, token, false, context.Exception.Message);

        }
    }

    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message)
        {

        }
    }

    public class ForbiddenException : Exception
    {
        public ForbiddenException(string message) : base(message)
        {

        }
    }

    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {

        }
    }

    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message)
        {

        }
    }
}
