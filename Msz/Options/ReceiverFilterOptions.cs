using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Msz.Options
{
    public class ReceiverFilterOptions
    {
        [Display(Name="Фамилия")]
        public string Surname { get; set; }
        [Display(Name = "Имя")]
        public string Name { get; set; }
        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }
        [Display(Name = "СНИЛС")]
        public string Snils { get; set; }
        [Display(Name = "Адрес")]
        public string Address { get; set; }
        public bool IncludeReasonPersonsInFiltering { get; set; }
        [Display(Name = "МСЗ")]
        public int? MszId { get; set; }
        [Display(Name = "Категория")]
        public int? CategoryId { get; set; }
        [Display(Name = "Дата решения")]
        public DateTime? DecisionDate { get; set; }
        [Display(Name = "Номер решения")]
        public string DecisionNumber { get; set; }
        [Display(Name = "Дата начала")]
        public DateTime? StartDate { get; set; }
        [Display(Name = "Дата окончания")]
        public DateTime? EndDate { get; set; }
        [Display(Name = "Дата изменения")]
        public DateTime? ModifyDate { get; set; }
        public bool CreateByMe { get; set; }

        public ReceiverFilterOptions()
        {
        }
    }
}
