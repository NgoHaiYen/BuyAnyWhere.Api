using Shopping.Contexts.Auth.Applications.Interfaces;
using Shopping.Models;
using System.Linq;
using System.Web.Http.Controllers;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Web.Http.Filters;
using Microsoft.Practices.Unity;
using Shopping.Ultilities;

namespace Shopping.App_Start
{
    public class WebApiAuthorization : ActionFilterAttribute
    {
        private readonly IUltilityService ultilityService;
        private readonly ShoppingEntities shoppingEntities;

        public WebApiAuthorization()
        {
            ultilityService = UnityConfig.GetConfiguredContainer().Resolve<IUltilityService>();
            this.shoppingEntities = UnityConfig.GetConfiguredContainer().Resolve<ShoppingEntities>();
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {

            string token = ultilityService.GetTokenFromHeaderHttpRequest(actionContext);

            string method = actionContext.Request.Method.ToString();
            string uri = ultilityService.NormalizePath(actionContext.Request.RequestUri.AbsolutePath);

            var publicApis = shoppingEntities.Apis.Where(t => t.Type == ApiTypeConstant.PUBLIC);

            if (publicApis.FirstOrDefault(t => t.Method == method && t.Uri == uri) != null)
            {
                return;
            }

            var userToken = shoppingEntities.UserTokens.FirstOrDefault(t => t.Name == token);
            var user = userToken.User;

            if (user.RoleId == null)
            {
                throw new UnauthorizedAccessException("Người dùng chưa được phân quyền");
            }

            var role = shoppingEntities.Roles.Include(t => t.Apis).First(t => t.Id == user.RoleId);

            if (role.Name == "Root")
            {
                return;
            }

            List<Api> apis = role.Apis.ToList();

            if (apis.FirstOrDefault(t => t.Method == method && t.Uri == uri) == null)
            {
                throw new UnauthorizedAccessException("Không thể truy cập đường dẫn này");
            }

        }
    }
}