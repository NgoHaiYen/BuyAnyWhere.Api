using System.Linq;
using Shopping.Models;
using Shopping.Ultilities;

namespace Shopping.Contexts.Procurement.Applications.Dtos
{
    public class ProductFilterDto : FilterDto<Product>
    {
        public override IQueryable<Product> ApplyTo(IQueryable<Product> source)
        {
            return Order(source);
        }
    }
}