using Shopping.Contexts.Auths.Applications.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Contexts.Auths.Applications.Interfaces
{
    public interface IRoleService
    {
        List<RoleDTO> Get();
    }
}
