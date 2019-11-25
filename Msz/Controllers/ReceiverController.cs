using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Msz.Models;
using Msz.Services;
using Msz.ViewModels;

namespace Msz.Controllers
{
    public class ReceiverController : Controller
    {
        private readonly IReceiverService _receiverSeriver;

        public ReceiverController(IReceiverService receiverSeriver)
        {
            _receiverSeriver = receiverSeriver;
        }

        public IActionResult Index(ReceiverIndexViewModel viewModel = null)
        {
            var resultViewModel = _receiverSeriver.GetIndexViewModel(viewModel);
            return View(resultViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
