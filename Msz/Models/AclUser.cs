using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Msz.Models
{
    public class AclUser
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Login { get; set; }
        public List<AclUserPrivilege> Privileges { get; set; }
    }
}
