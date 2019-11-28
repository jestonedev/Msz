using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Msz.Models
{
    public class AclUserPrivilege
    {
        public int UserId { get; set; }
        public AclUser User { get; set; }
        public int PrivilegeId { get; set; }
        public AclPrivilege Privilege { get; set; }
        public int MszId { get; set; }
        public Msz Msz { get; set; }
    }
}
