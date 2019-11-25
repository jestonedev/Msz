using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Msz.Models
{
    public class Receiver
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
        public string Address { get; set; }
        public string Phone { get; set; }

        public int MszId { get; set; }
        public Msz Msz { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public DateTime DecisionDate { get; set; }
        public string DecisionNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int AssigmentFormId { get; set; }
        public AssigmentForm AssigmentForm { get; set; }

        public decimal Amount { get; set; }
        public decimal? EquivalentAmount { get; set; }

        public List<ReasonPerson> ReasonPersons { get; set; }

        [Required]
        public string Uuid { get; set; }

        public int? PrevRevisionId { get; set; }
        public int? NextRevisionId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
