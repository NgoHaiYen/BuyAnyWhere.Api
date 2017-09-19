using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Contexts.Auth.Applications.Interfaces
{
    public interface IAppService
    {
        string GetTokenFromHeaderHttpRequest(object context);
        string NormalizePath(string path);
    }
}
