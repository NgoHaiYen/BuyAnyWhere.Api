using Newtonsoft.Json;
using Shopping.Contexts.Auth.Applications.DTOs;
using Shopping.Contexts.Auth.Applications.Interfaces;
using Shopping.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace Shopping.App_Start
{
    public class ExceptionResponeAttribute : ExceptionFilterAttribute
    {
        private readonly ShoppingEntities shoppingEntities;
        private readonly IUltilityService appService;

        public ExceptionResponeAttribute(ShoppingEntities shoppingEntities, IUltilityService appService)
        {
            this.shoppingEntities = shoppingEntities;
            this.appService = appService;
        }

        public override void OnException(HttpActionExecutedContext Context)
        {
            var Response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            if (Context.Exception is BadRequestException)
            {
                Response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                Response.ReasonPhrase = "Wrong paramenters";

            }
            if (Context.Exception is ConflictException)
            {
                Response = new HttpResponseMessage(HttpStatusCode.Conflict);
                Response.ReasonPhrase = "Conflict data";
            }

            if (Context.Exception is ForbiddenException)
            {
                Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                Response.ReasonPhrase = "Forbidden";
            }
            if (Context.Exception is NotFoundException)
            {
                Response = new HttpResponseMessage(HttpStatusCode.NotFound);
                Response.ReasonPhrase = "Not Found";
            }

            if (Context.Exception is DataException)
            {
                Response = new HttpResponseMessage(HttpStatusCode.NotModified);
                Response.ReasonPhrase = "Not Modified Data";
            }

            var Message = JsonConvert.SerializeObject(new { Message = Context.Exception.Message });

            Response.Content = new StringContent(Message);
            Context.Response = Response;


            var token = appService.GetTokenFromHeaderHttpRequest(Context);
            User user = appService.GetUserFromTokenAlwayReturnUserName(token);



        }
    }

    public class BadRequestException : Exception
    {
        public BadRequestException(string Message) : base(Message)
        {

        }
    }

    public class ForbiddenException : Exception
    {
        public ForbiddenException(string Message) : base(Message)
        {

        }
    }

    public class NotFoundException : Exception
    {
        public NotFoundException(string Message) : base(Message)
        {

        }
    }

    public class ConflictException : Exception
    {
        public ConflictException(string Message) : base(Message)
        {

        }
    }
}
