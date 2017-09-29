using System.Linq;
using Shopping.Models;
using Shopping.Ultilities;

namespace Shopping.Contexts.Auth.Applications.DTOs
{
    public class UserFilterDto : FilterDto<User>
    {
        public string Name { get; set; }

        public override IQueryable<User> ApplyTo(IQueryable<User> source)
        {
            if (Name != null)
            {
                source = source.Where(t => t.Name == Name);
            }

            return Order(source);
        }
    }
}