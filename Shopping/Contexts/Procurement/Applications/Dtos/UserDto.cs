using Shopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopping.Contexts.Procurement.Applications.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FacebookId { get; set; }
        public string GoogleId { get; set; }

        public List<CategoryDto> FavoriteCategoryDtos { get; set; }
        public List<PurchaseOrderDto> PurchaseOrderDtos { get; set; }

        public UserDto() { }

        public UserDto(User user, params object[] args)
        {
            this.Id = user.Id;
            this.Name = user.Name;
            this.FacebookId = user.FacebookId;
            this.GoogleId = user.GoogleId;

            foreach (var arg in args)
            {
                if (arg is ICollection<Category> categories)
                {
                    FavoriteCategoryDtos = new List<CategoryDto>();
                    foreach(var t in categories)
                    {
                        FavoriteCategoryDtos.Add(new CategoryDto(t));
                    }
                }
                else if (arg is ICollection<PurchaseOrder> purchaseOrders)
                {
                    PurchaseOrderDtos = new List<PurchaseOrderDto>();
                    foreach(var t in purchaseOrders)
                    {
                        PurchaseOrderDtos.Add(new PurchaseOrderDto(t));
                    }
                }
            }
        }

        public User ToModel(User user = null)
        { 
            if (user == null) {
                user = new User();
                user.Id = Guid.NewGuid();
            }

            user.Name = Name;
            user.FacebookId = FacebookId;
            user.GoogleId = GoogleId;

            return user;
        }
    }
}