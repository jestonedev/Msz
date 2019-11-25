using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Msz.Options
{
    public class ReceiverFilterOptions
    {
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string Snils { get; set; }
        public string Address { get; set; }
        public bool IncludeReasonPersonsInFiltering { get; set; }
        public int? MszId { get; set; }
        public int? CategoryId { get; set; }
        public DateTime? DecisionDate { get; set; }
        public string DecisionNumber { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ModifyDate { get; set; }

        public ReceiverFilterOptions()
        {
        }
    }
}
