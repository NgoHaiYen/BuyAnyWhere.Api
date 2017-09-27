using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Contexts.Procurement.Applications.Interfaces
{
    public interface INotificationService
    {
        void Notify(string title, string body, string cloudToken);
    }
}
