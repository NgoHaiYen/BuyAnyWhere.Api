using Shopping.Contexts.Auth.Applications.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Contexts.Auth.Applications.Interfaces
{
    public interface IRoleService
    {
        List<RoleDto> Get();
    }
}
