using Shopping.Contexts.Auth.Applications.DTOs;
using Shopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopping.Contexts.Procurement.Applications.Dtos
{
    public class FavoriteCategoryDto
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid? CategoryId { get; set; }
        public CategoryDto CategoryDto { get; set; }
        public UserDto UserDto { get; set; }

        public FavoriteCategoryDto(FavoriteCategory favoriteCategoryDto, params object[] args)
        {
            Id = favoriteCategoryDto.Id;
            UserId = favoriteCategoryDto.UserId;
            CategoryId = favoriteCategoryDto.CategoryId;

            foreach (var arg in args)
            {
                if (arg is User user)
                {
                    UserDto = new UserDto(user);
                } 
                else if (arg is Category category)
                {
                    CategoryDto = new CategoryDto(category);
                }
            }
        }

        public FavoriteCategory ToModel(FavoriteCategory favoriteCategory = null)
        {
            if (favoriteCategory == null)
            {
                favoriteCategory = new FavoriteCategory();
                favoriteCategory.Id = Guid.NewGuid();
            }

            favoriteCategory.UserId = UserId;
            favoriteCategory.CategoryId = CategoryId;

            return favoriteCategory;
        }
    }
}