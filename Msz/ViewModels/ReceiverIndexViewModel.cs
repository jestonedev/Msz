using Msz.Models;
using Msz.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Msz.ViewModels
{
    public class ReceiverIndexViewModel
    {
        public ReceiverFilterOptions FilterOptions { get; set; }
        public PageOptions PageOptions { get; set; }
        public List<Receiver> Receivers { get; set; }
        public List<Models.Msz> Mszs { get; set; }
        public List<Category> Categories { get; set; }
    }
}
