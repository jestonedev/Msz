using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Msz.Services
{
    public class AclService: IAclService
    {
        public AclService()
        {
        }

        public string GetLogin()
        {
            return Helpers.AppContext.Current.User?.Identity?.Name;
        }
    }
}
