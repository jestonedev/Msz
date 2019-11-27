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
        [Required(ErrorMessage = "Поле является обязательным для заполнения")]
        [Display(Name="Фамилия")]
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
        [Display(Name = "Адрес")]
        public string Address { get; set; }
        [Display(Name = "Телефон")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Поле является обязательным для заполнения")]
        [Display(Name = "МСЗ")]
        public int MszId { get; set; }
        public Msz Msz { get; set; }
        [Required(ErrorMessage = "Поле является обязательным для заполнения")]
        [Display(Name = "Категория")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        [Required(ErrorMessage = "Поле является обязательным для заполнения")]
        [Display(Name = "Дата решения")]
        public DateTime DecisionDate { get; set; }
        [Display(Name = "Номер решения")]
        public string DecisionNumber { get; set; }
        [Required(ErrorMessage = "Поле является обязательным для заполнения")]
        [Display(Name = "Дата начала")]
        public DateTime StartDate { get; set; }
        [Display(Name = "Дата окончания")]
        public DateTime? EndDate { get; set; }
        [Required(ErrorMessage = "Поле является обязательным для заполнения")]
        [Display(Name = "Форма предоставления")]
        public int AssigmentFormId { get; set; }
        public AssigmentForm AssigmentForm { get; set; }
        [Required(ErrorMessage = "Поле является обязательным для заполнения")]
        [Display(Name = "Размер (кол-во, сумма)")]
        public decimal Amount { get; set; }
        [Display(Name = "Эквивалентная сумма")]
        public decimal? EquivalentAmount { get; set; }

        public List<ReasonPerson> ReasonPersons { get; set; }

        [Required]
        public string Uuid { get; set; }

        public int? PrevRevisionId { get; set; }
        public int? NextRevisionId { get; set; }
        public DateTime CreatedDate { get; set; }
        [Required]
        public string Creator { get; set; }
        public bool IsDeleted { get; set; }
    }
}
