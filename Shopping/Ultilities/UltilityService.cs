﻿using System;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Shopping.Contexts.Auth.Applications.Interfaces;
using Shopping.Models;

namespace Shopping.Ultilities
{
    public class UltilityService : IUltilityService
    {
        private readonly ShoppingEntities shoppingEntities;

        public UltilityService(ShoppingEntities shoppingEntities)
        {
            this.shoppingEntities = shoppingEntities;
        }

        public string GetTokenFromHeaderHttpRequest(HttpAuthenticationContext context)
        {
            if (context.Request.Headers.Contains(Constant.ACCESS_TOKEN))
            {
                var token = context.Request.Headers.GetValues(Constant.ACCESS_TOKEN).First();
                return token;
            }

            return null;
        }

        public string GetTokenFromHeaderHttpRequest(HttpActionExecutedContext context)
        {
            if (context.Request.Headers.Contains(Constant.ACCESS_TOKEN))
            {
                var token = context.Request.Headers.GetValues(Constant.ACCESS_TOKEN).First();
                return token;
            }

            return null;
        }

        public string GetTokenFromHeaderHttpRequest(HttpContext context)
        {
            if (context.Request.Headers[Constant.ACCESS_TOKEN] != null)
            {
                var token = (context.Request.Headers.GetValues(Constant.ACCESS_TOKEN) ?? throw new InvalidOperationException()).First();
                return token;
            }

            return null;
        }

        public string GetTokenFromHeaderHttpRequest(HttpActionContext context)
        {
            if (context.Request.Headers.Contains(Constant.ACCESS_TOKEN))
            {
                var token = context.Request.Headers.GetValues(Constant.ACCESS_TOKEN).First();
                return token;
            }

            return null;
        }

        public User GetUserFromTokenAlwayReturnUserName(string token)
        {
            User user = null;

            if (token == null)
            {
                user = new User {Name = "Khách vãng lai"};
            }
            else
            {
                var userToken = shoppingEntities.UserTokens.FirstOrDefault(t => t.Name == token);

                if (userToken == null)
                {
                    user = new User {Name = "Người dùng sai Token"};
                }

                if (userToken != null) user = userToken.User;
            }

            return user;
        }


        public void Log(object context, string headerToken, bool success, string reason)
        {
            dynamic dynamicContect = context;

            var logger = new Logger
            {
                Id = Guid.NewGuid(),
                UserName = GetUserFromTokenAlwayReturnUserName(headerToken).Name,
                DateTime = DateTime.Now,
                ApiMethod = dynamicContect.Request.Method.ToString(),
                ApiUri = dynamicContect.Request.RequestUri.AbsolutePath,
                Success = success,
                Reason = reason
            };

            shoppingEntities.Loggers.Add(logger);
            shoppingEntities.SaveChanges();
        }


        public string NormalizePath(string path)
        {
            var linkParts = path.Split('/').Where(x => !string.IsNullOrEmpty(x)).ToArray();

            for (var i = 0; i < linkParts.Length; i++)
            {
                if (Guid.TryParse(linkParts[i], out _))
                    linkParts[i] = "*";
            }
            return string.Join("/", linkParts);
        }
    }
}