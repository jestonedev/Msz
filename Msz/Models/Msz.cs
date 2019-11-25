using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Msz.Models
{
    public class Msz
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Guid { get; set; }
        public List<Category> Categories { get; set; }
        public int? PreviousRevisionId { get; set; }
        public int? NextRevisionId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
