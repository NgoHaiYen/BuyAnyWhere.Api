using System.Linq;
using Shopping.Models;
using Shopping.Ultilities;

namespace Shopping.Contexts.Auth.Applications.DTOs
{
    public class UserFilterDto : FilterDto<User>
    {
        public string Name { get; set; }
        public string RoleName { get; set; }

        public override IQueryable<User> ApplyTo(IQueryable<User> source)
        {
            if (Name != null)
            {
                source = source.Where(t => t.Name == Name);
            }

            if (RoleName != null)
            {
                source = source.Where(t => t.Role.Name == RoleName);
            }

            return Order(source);
        }
    }
}