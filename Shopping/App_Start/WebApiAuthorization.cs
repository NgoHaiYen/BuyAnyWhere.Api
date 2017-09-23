using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Microsoft.Practices.Unity;
using Shopping.App_Start;
using Shopping.Contexts.Auth.Applications.Interfaces;
using Shopping.Models;
using Shopping.Ultilities;

namespace Shopping
{
    public class WebApiAuthorization : ActionFilterAttribute
    {
        private readonly ShoppingEntities shoppingEntities;
        private readonly IUltilityService ultilityService;

        public WebApiAuthorization()
        {
            ultilityService = UnityConfig.GetConfiguredContainer().Resolve<IUltilityService>();
            shoppingEntities = UnityConfig.GetConfiguredContainer().Resolve<ShoppingEntities>();
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var token = ultilityService.GetTokenFromHeaderHttpRequest(actionContext);
            var method = actionContext.Request.Method.ToString();
            var uri = ultilityService.NormalizePath(actionContext.Request.RequestUri.AbsolutePath);

            var publicApis = shoppingEntities.Apis.Where(t => t.Type == (int) Constant.TypeApi.Public);


            // Nếu là PUBLIC API thì thành công

            if (publicApis.FirstOrDefault(t => t.Method == method && t.Uri == uri) != null)
            {
                return;
            }

            // Nếu là PRIVATE API thì ... 

            var userToken = shoppingEntities.UserTokens.First(t => t.Name == token);
            var user = userToken.User;
            var role = shoppingEntities.Roles.Include(t => t.Apis).First(t => t.Id == user.RoleId);


            if (role == null)
            {
                throw new UnauthorizedAccessException("Người dùng chưa được phân quyền");
            }

            if (role.Name == "Root")
            {
                return;
            }

            var apis = role.Apis.ToList();

            if (apis.FirstOrDefault(t => t.Method == method && t.Uri == uri) == null)
            {
                throw new UnauthorizedAccessException("Không thể truy cập đường dẫn này");
            }
        }
    }
}