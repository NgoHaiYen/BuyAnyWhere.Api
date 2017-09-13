using Shopping.Applications.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Applications.Interfaces
{
    public interface IUserService
    {
        List<UserDTO> Get();
        UserDTO Create(UserDTO userDTO);
        UserDTO Get(Guid id);
    }
}
