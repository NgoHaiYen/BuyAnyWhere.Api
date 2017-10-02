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

        [HttpPut]
        [Route("current/FavoriteCategories/{categoryId}")]
        public IHttpActionResult PostCurrentUserFavoriteCategory([FromUri] Guid categoryId)
        {
            var token = ultilityService.GetHeaderToken(HttpContext.Current);

            var userToken = shoppingEntities.UserTokens.FirstOrDefault(t => t.Name == token);

            if (userToken == null)
            {
                throw new BadRequestException("Access token khong hop le");
            }

            var user = userToken.User;

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
        [Route("current/FavoriteCategories")]
        public IHttpActionResult GetFavoriteCategories()
        {

            var token = ultilityService.GetHeaderToken(HttpContext.Current);

            var userToken = shoppingEntities.UserTokens.FirstOrDefault(t => t.Name == token);

            if (userToken == null)
            {
                throw new BadRequestException("Access token khong hop le");
            }

            var user = userToken.User;

            var favoriteCategories = user.FavoriteCategories.Select(t => t.Category).ToList();

            var favoriteCategoryDtos = favoriteCategories.ConvertAll(t => new CategoryDto(t));

            return Ok(favoriteCategoryDtos);
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
        public IHttpActionResult GetCurrentUserAllPurchaseOrders()
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

        [HttpPost]
        [Route("current/PurchaseOrders")]
        public IHttpActionResult PostCurrentUserPurchaseOrder([FromBody] PurchaseOrderDto purchaseOrderDto)
        {
            var token = ultilityService.GetHeaderToken(HttpContext.Current);

            var userToken = shoppingEntities.UserTokens.FirstOrDefault(t => t.Name == token);

            if (userToken == null)
            {
                throw new BadRequestException("Access token khong hop le");
            }

            var user = userToken.User;

            using (ShoppingEntities shoppingEntities = new ShoppingEntities())
            {
                using (var transaction = shoppingEntities.Database.BeginTransaction())
                {
                    try
                    {
                        var purchaseOrder = purchaseOrderDto.ToModel();
                        purchaseOrder.WorkFlowStatus = (int)Constant.Status.Running;
                        purchaseOrder.WorkFlowLevel = 1;

                        List<PurchaseOrderDetail> purchaseOrderDetails = purchaseOrderDto.PurchaseOrderDetailDtos.ConvertAll(t => t.ToModel());

                        shoppingEntities.PurchaseOrders.Add(purchaseOrder);
                        shoppingEntities.PurchaseOrderDetails.AddRange(purchaseOrderDetails);

                        shoppingEntities.PurchaseOrderWorkFlows.Add(
                            new PurchaseOrderWorkFlow {
                                Id = Guid.NewGuid(),
                                UserId = user.Id,
                                Level = 1,
                                Status = (int)Constant.WorkFlowStatus.Approved,
                                Reason = "",
                                PurchaseOrderId = purchaseOrder.Id
                            });

                        shoppingEntities.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                    }
                }
            }

            return Ok();
        }

        //[HttpDelete]
        //[Route("current/PurchaseOrders/{purchaseOrderId}")]
        //public IHttpActionResult Delete([FromUri] Guid purchaseOrderId)
        //{
        //    var token = ultilityService.GetHeaderToken(HttpContext.Current);

        //    var userToken = shoppingEntities.UserTokens.FirstOrDefault(t => t.Name == token);

        //    if (userToken == null)
        //    {
        //        throw new BadRequestException("Access token khong hop le");
        //    }

        //    var user = userToken.User;

        //    var purchaseOrder = shoppingEntities.PurchaseOrders
        //        .Include(t => t.PurchaseOrderDetails)
        //        .Include(t => t.PurchaseOrderWorkFlows)
        //        .FirstOrDefault(t => t.BuyerId == user.Id && t.Id == purchaseOrderId
        //        && t.WorkFlowLevel == 1);

        //    if (purchaseOrder == null)
        //    {
        //        throw new BadRequestException("PurchaseOrder không hợp lệ");
        //    }

        //    purchaseOrder.WorkFlowStatus = (int)Constant.PurchaseOrderWorkFlowStatus.Rejected;


        //    PurchaseOrderWorkFlow purchaseOrderWork = new PurchaseOrderWorkFlow();
        //    purchaseOrderWork.Id = Guid.NewGuid();
        //    purchaseOrderWork.

        //    return Ok();
        //}


        [HttpGet]
        [Route("current/PurchaseOrders/Deleted")]
        public IHttpActionResult GetCurrentUserDeletedPurchaseOrders()
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
                    && t.WorkFlowStatus == (int)Constant.WorkFlowStatus.Rejected)
                .ToList();

            var purchaseOrderDtos = purchaseOrders.ConvertAll(t => new PurchaseOrderDto(t, t.PurchaseOrderDetails, t.PurchaseOrderWorkFlows));

            return Ok(purchaseOrderDtos);
        }
    }
}
