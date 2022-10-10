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
        [Required(ErrorMessage = "Поле является обязательным для заполнения")]
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Поле является обязательным для заполнения")]
        [Display(Name = "Имя")]
        public string Name { get; set; }
        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }
        [Required(ErrorMessage = "Поле является обязательным для заполнения")]
        [Display(Name = "Пол")]
        public int GenderId { get; set; }
        public Gender Gender { get; set; }
        [Required(ErrorMessage = "Поле является обязательным для заполнения")]
        [Display(Name = "Дата рождения")]
        public DateTime BirthDate { get; set; }
        [Required(ErrorMessage = "Поле является обязательным для заполнения")]
        [Display(Name = "СНИЛС")]
        [RegularExpression("[0-9]{3}-[0-9]{3}-[0-9]{3}-[0-9]{2}", ErrorMessage = "Указано некорректно значение")]
        public string Snils { get; set; }
        public int ReceiverId { get; set; }
        public Receiver Receiver { get; set; }

        [Required(ErrorMessage = "Поле является обязательным для заполнения")]
        [Display(Name = "Родственная связь")]
        public int KinshipRelationId { get; set; }
        public KinshipRelation KinshipRelation { get; set; }
    }
}
