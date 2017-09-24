using Shopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopping.Contexts.Procurement.Applications.Dtos
{
    public class ShopDto
    {
        public Guid Id { get; set; }
        public Guid? OwnerId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Description { get; set; }
        public decimal? Rating { get; set; }
        public DateTime? CreatedDate { get; set; }


        public ShopDto(Shop shop, params object[] args)
        {
            Id = shop.Id;
            OwnerId = shop.OwnerId;
            Name = shop.Name;
            Phone = shop.Phone;
            Email = shop.Email;
            Website = shop.Website;
            Description = shop.Description;
            Rating = shop.Rating;
            CreatedDate = shop.CreatedDate;
        }

        public Shop ToModel(Shop shop = null)
        {
            if (shop == null)
            {
                shop = new Shop();
                shop.Id = Guid.NewGuid();
            }

            shop.OwnerId = OwnerId;
            shop.Name = Name;
            shop.Phone = Phone;
            shop.Email = Email;
            shop.Website = Website;
            shop.Description = Description;
            shop.Rating = Rating;
            shop.CreatedDate = CreatedDate;

            return shop;
        }
    }
}