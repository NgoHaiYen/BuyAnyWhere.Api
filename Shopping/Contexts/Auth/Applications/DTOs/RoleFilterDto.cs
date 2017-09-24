using Shopping.Models;
using Shopping.Ultilities;
using System.Linq;

namespace Shopping.Contexts.Auth.Applications.DTOs
{
    public class RoleFilterDto : FilterDto<Role>
    {
        public string Name { get; set; }

        public override IQueryable<Role> ApplyTo(IQueryable<Role> source)
        {
            if (! string.IsNullOrEmpty(Name))
            {
                source = source.Where(t => t.Name == Name);
            }

            return Order(source);
        }
    }
}