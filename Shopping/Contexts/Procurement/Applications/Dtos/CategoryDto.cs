using Shopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopping.Contexts.Procurement.Applications.Dtos
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public CategoryDto(Category category)
        {
            Id = category.Id;
            Name = category.Name;
            Description = category.Description;
        }


        public Category ToModel(Category category = null)
        {
            if (category == null)
            {
                category = new Category();
                category.Id = Guid.NewGuid();
            }

            category.Name = Name;
            category.Description = Description;

            return category;
        }
    }
}