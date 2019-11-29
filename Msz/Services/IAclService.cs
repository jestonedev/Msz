using Msz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Msz.Services
{
    public interface IAclService
    {
        string GetLogin();
        AclUser GetUser();
        List<int> GetAllowedMszs(AclUser user);
        bool CanUpdate(Receiver reciever);
        bool CanDelete(Receiver reciever);
        bool CanInsert(Receiver reciever);
        bool CanInsertAny();
        bool CanReadAny();
    }
}
