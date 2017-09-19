﻿using Shopping.Contexts.Auth.Applications.Interfaces;
using Shopping.Models;
using Shopping.Ultilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shopping.Contexts.Auth.Applications
{
    public class UltilityService : IUltilityService
    {

        private readonly ShoppingEntities shoppingEntities;

        public UltilityService(ShoppingEntities shoppingEntities)
        {
            this.shoppingEntities = shoppingEntities;
        }

        public string GetTokenFromHeaderHttpRequest(object context) 
        {
            string token = null;
            dynamic ctx = context;

            if (ctx.Request.Headers.TryGetValues(RequestConstant.ACCESS_TOKEN, out IEnumerable<string> values))
            {
                token = values.First();
            }

            return null;
        }

        public User GetUserFromTokenAlwayReturnUserName(string token)
        {

            User user = null;

            // Khong co token, tra ve thong tin User mac dinh
            if (token == null)
            {
                user = new User();
                user.Name = "GUEST";

            } else
            {
                var userToken = shoppingEntities.UserTokens.FirstOrDefault(t => t.Name == token);

                // token sai hoac qua han roi, truy cap public api nen van duoc
                if (userToken == null)
                {
                    user = new User();
                    user.Name = "INVALID_TOKEN_USER";
                }

                user = userToken.User;          
            }

            return user;
        }


        public void Log(object context, string userToken, bool success, string reason)
        {
            dynamic ctx = context;

            Logger logger = new Logger();
            logger.Id = Guid.NewGuid();
            logger.UserName = GetUserFromTokenAlwayReturnUserName(userToken).Name;
            logger.DateTime = System.DateTime.Now;
            logger.ApiMethod = ctx.Request.Method.ToString();
            logger.ApiUri = ctx.Request.RequestUri.AbsolutePath;
            logger.Success = success;
            logger.Reason = reason;

            shoppingEntities.Loggers.Add(logger);
            shoppingEntities.SaveChanges();
        }


        public string NormalizePath(string path)
        {
            string[] linkParts = path.Split('/').Where(x => !string.IsNullOrEmpty(x)).ToArray();

            for (int i = 0; i < linkParts.Length; i++)
            {
                Guid testGuid;
                if (Guid.TryParse(linkParts[i], out testGuid))
                {
                    linkParts[i] = "*";
                }
            }
            return String.Join("/", linkParts);
        }
    }
}