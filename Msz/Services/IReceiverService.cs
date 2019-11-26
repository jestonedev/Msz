using Msz.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Msz.Services
{
    public interface IReceiverService
    {
        ReceiverIndexViewModel GetIndexViewModel(ReceiverIndexViewModel viewModel = null);
        bool UpdateMszAndCategories(XDocument xml);

        ReceiverViewModel GetEmptyViewModel();
    }
}
