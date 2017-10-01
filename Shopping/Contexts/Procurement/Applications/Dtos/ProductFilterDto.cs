using System.Linq;
using Shopping.Models;
using Shopping.Ultilities;

namespace Shopping.Contexts.Procurement.Applications.Dtos
{
    public class ProductFilterDto : FilterDto<Product>
    {
        public string Name { get; set; }

        public override IQueryable<Product> ApplyTo(IQueryable<Product> source)
        {
            if (Name != null)
            {
                source = source.Where(t => t.Name.ToLower().Contains(Name.ToLower()));
            }

            return Order(source);
        }
    }
}