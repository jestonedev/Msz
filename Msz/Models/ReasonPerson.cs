using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Msz.Models
{
    public class ReasonPerson
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public int GenderId { get; set; }
        public Gender Gender { get; set; }
        public DateTime BirthDate { get; set; }
        [Required]
        public string Snils { get; set; }
        public int ReceiverId { get; set; }
        public Receiver Receiver { get; set; }
    }
}
