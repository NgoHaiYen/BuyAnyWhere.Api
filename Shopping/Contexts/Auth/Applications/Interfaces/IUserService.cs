using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shopping.Contexts.Auth.Applications.DTOs;
using Shopping.Ultilities;

namespace Shopping.Applications.Interfaces
{
    public interface IUserService
    {
        List<UserDto> Get(PaginateDto paginateDto);
        UserDto Create(UserDto userDTO);
        UserDto Get(Guid id);
    }
}
