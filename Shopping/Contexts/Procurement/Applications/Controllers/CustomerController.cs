using Shopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using Shopping.Contexts.Procurement.Applications.Dtos;
using Shopping.Contexts.Auth.Applications.Interfaces;
using System.Web;
using Shopping.Ultilities;

namespace Shopping.Contexts.Procurement.Applications.Controllers
{

    [RoutePrefix("api/Procurement/Users")]
    public class CustomerController : ApiController
    {
        private readonly ShoppingEntities shoppingEntities;
        private readonly IUltilityService ultilityService;

        public CustomerController(ShoppingEntities shoppingEntities, IUltilityService ultilityService)
        {
            this.shoppingEntities = shoppingEntities;
            this.ultilityService = ultilityService;
        }

        [HttpPut]
        [Route("{userId}/FavoriteCategories/{categoryId}")]
        public IHttpActionResult PostFavoriteCategory([FromUri] Guid userId, [FromUri] Guid categoryId)
        {
            var user = shoppingEntities.Users.FirstOrDefault(t => t.Id == userId);
            var category = shoppingEntities.Categories.FirstOrDefault(t => t.Id == categoryId);

            if (user == null)
            {
                throw new BadRequestException("User khong ton tai");
            }

            if (category == null)
            {
                throw new BadRequestException("Category khong ton tai");
            }

            if (shoppingEntities.FavoriteCategories.FirstOrDefault(t => t.UserId == user.Id && t.CategoryId == category.Id) == null)
            {
                FavoriteCategory favoriteCategory = new FavoriteCategory();
                favoriteCategory.Id = Guid.NewGuid();
                favoriteCategory.Category = category;
                favoriteCategory.User = user;
                shoppingEntities.FavoriteCategories.Add(favoriteCategory);

                shoppingEntities.SaveChanges();
            } 


            return Ok(new CategoryDto(category));
        }

        [HttpDelete]
        [Route("{userId}/FavoriteCategories/{categoryId}")]
        public IHttpActionResult DeleteFavoriteCategory([FromUri] Guid userId, [FromUri] Guid categoryId)
        {
            var user = shoppingEntities.Users.FirstOrDefault(t => t.Id == userId);
            var category = shoppingEntities.Categories.FirstOrDefault(t => t.Id == categoryId);

            if (user == null)
            {
                throw new BadRequestException("User khong ton tai");
            }

            if (category == null)
            {
                throw new BadRequestException("Category khong ton tai");
            }

            FavoriteCategory favoriteCategory = shoppingEntities.FavoriteCategories.FirstOrDefault(t => t.UserId == userId && t.CategoryId == categoryId);

            if (favoriteCategory == null)
            {
                throw new BadRequestException("User chua thich Category nay");
            }

            shoppingEntities.FavoriteCategories.Remove(favoriteCategory);
            shoppingEntities.SaveChanges();

            return Ok(new CategoryDto(category));
        }

        [HttpGet]
        [Route("{userId}/FavoriteCategories")]
        public IHttpActionResult GetFavoriteCategories([FromUri] Guid userId)
        {
            var users = shoppingEntities.Users.Include(t => t.FavoriteCategories.Select(u => u.Category)).FirstOrDefault(t => t.Id == userId);

            var favoriteCategories = users.FavoriteCategories.Select(t => t.Category).ToList();

            var favoriteCategoryDtos = favoriteCategories.ConvertAll(t => new CategoryDto(t));

            return Ok(favoriteCategoryDtos);
        }

        [HttpGet]
        [Route("current/PurchaseOrders/All")]
        public IHttpActionResult GetCurrentAllPurchaseOrders()
        {
            var token = ultilityService.GetHeaderToken(HttpContext.Current);

            var userToken = shoppingEntities.UserTokens.FirstOrDefault(t => t.Name == token);

            if (userToken == null)
            {
                throw new BadRequestException("Access token khong hop le");
            }

            var user = userToken.User;

            var purchaseOrders = shoppingEntities.PurchaseOrders.Include(t => t.PurchaseOrderDetails)
                .Include(t => t.PurchaseOrderWorkFlows).Where(t => t.BuyerId == user.Id).ToList();

            var purchaseOrderDtos = purchaseOrders.ConvertAll(t => new PurchaseOrderDto(t, t.PurchaseOrderDetails, t.PurchaseOrderWorkFlows));

            return Ok(purchaseOrderDtos);
        }

        [HttpGet]
        [Route("current/PurchaseOrders/Deleted")]
        public IHttpActionResult GetCurrentDeletedPurchaseOrders()
        {
            var token = ultilityService.GetHeaderToken(HttpContext.Current);

            var userToken = shoppingEntities.UserTokens.FirstOrDefault(t => t.Name == token);

            if (userToken == null)
            {
                throw new BadRequestException("Access token khong hop le");
            }

            var user = userToken.User;

            var purchaseOrders = shoppingEntities.PurchaseOrders
                .Include(t => t.PurchaseOrderDetails)
                .Include(t => t.PurchaseOrderWorkFlows)
                .Where(t => t.BuyerId == user.Id && t.WorkFlowLevel == 1 
                    && t.WorkFlowStatus == (int)Constant.PurchaseOrderWorkFlowStatus.Rejected)
                .ToList();

            var purchaseOrderDtos = purchaseOrders.ConvertAll(t => new PurchaseOrderDto(t, t.PurchaseOrderDetails, t.PurchaseOrderWorkFlows));

            return Ok(purchaseOrderDtos);
        }
    }
}
