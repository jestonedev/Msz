using Msz.DatabaseContext;
using Msz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Msz.Services
{
    public class AclService: IAclService
    {
        private readonly IMszDbContext _dbContext;
        private AclUser currentUser = null;
        private List<int> allowedMszs = null;

        public AclService(IMszDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public string GetLogin()
        {
            return Helpers.AppContext.Current.User?.Identity?.Name;
        }

        public AclUser GetUser()
        {
            if (currentUser != null) return currentUser;
            var login = GetLogin();
            if (string.IsNullOrEmpty(login)) return currentUser;
            currentUser = _dbContext.AclUsers.Include(r => r.Privileges).FirstOrDefault(r => r.Login.ToUpperInvariant() == login.ToUpperInvariant());
            return currentUser;
        }

        public bool CanUpdate(Receiver reciever)
        {
            if (reciever == null) return false;
            var user = GetUser();
            if (user == null) return false;
            var allowedMszs = GetAllowedMszs(user);
            return reciever.IsDeleted == false && reciever.NextRevisionId == null && allowedMszs.Contains(reciever.MszId);
        }

        public List<int> GetAllowedMszs(AclUser user)
        {
            if (allowedMszs != null) return allowedMszs;
            var mszIds = user.Privileges.Where(r => r.PrivilegeId == 2).Select(r => r.MszId).ToList();
            allowedMszs = new List<int>();
            foreach(var mszId in mszIds)
            {
                var localMsz = _dbContext.Mszs.FirstOrDefault(r => r.Id == mszId);
                allowedMszs.Add(localMsz.Id);
                while (localMsz?.PreviousRevisionId != null)
                {
                    localMsz = _dbContext.Mszs.FirstOrDefault(r => r.Id == localMsz.PreviousRevisionId);
                    if (localMsz != null)
                    {
                        allowedMszs.Add(localMsz.Id);
                    }
                }
                localMsz = _dbContext.Mszs.FirstOrDefault(r => r.Id == mszId);
                while (localMsz?.NextRevisionId != null)
                {
                    localMsz = _dbContext.Mszs.FirstOrDefault(r => r.Id == localMsz.NextRevisionId);
                    if (localMsz != null)
                    {
                        allowedMszs.Add(localMsz.Id);
                    }
                }
            }
            return allowedMszs;
        }

        public bool CanDelete(Receiver reciever)
        {
            return CanUpdate(reciever);
        }

        public bool CanInsertAny()
        {
            var user = GetUser();
            if (user == null) return false;
            return user.Privileges.Where(r => r.PrivilegeId == 2).Any();
        }

        public bool CanInsert(Receiver reciever)
        {
            if (reciever == null) return false;
            var user = GetUser();
            if (user == null) return false;
            var allowedMszs = GetAllowedMszs(user);
            return allowedMszs.Contains(reciever.MszId);
        }


        public bool CanReadAny()
        {
            var user = GetUser();
            return user != null;
        }
    }
}
