using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shopping.Contexts.Auth.Applications.DTOs;
using Shopping.Contexts.Auths.Applications.DTOs;

namespace Shopping.Applications.Interfaces
{
    public interface IUserService
    {
        List<UserDto> Get();
        UserDto Create(UserDto userDTO);
        UserDto Get(Guid id);
    }
}
