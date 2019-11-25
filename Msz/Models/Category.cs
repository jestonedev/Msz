using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Msz.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public int MszId { get; set; }

        public virtual Msz Msz { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Guid { get; set; }
    }
}
