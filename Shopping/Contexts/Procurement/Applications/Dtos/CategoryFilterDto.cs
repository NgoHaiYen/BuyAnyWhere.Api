using Shopping.Models;
using Shopping.Ultilities;
using System.Linq;

namespace Shopping.Contexts.Procurement.Applications.Dtos
{
    public class CategoryFilterDto : FilterDto<Category>
    {
        public override IQueryable<Category> ApplyTo(IQueryable<Category> source)
        {

            return Order(source);
        }
    }
}