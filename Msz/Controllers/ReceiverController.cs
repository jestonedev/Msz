using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Msz.Models;
using Msz.Services;
using Msz.ViewModels;
using System.Xml.XPath;

namespace Msz.Controllers
{
    public class ReceiverController : Controller
    {
        private readonly IReceiverService _receiverService;

        public ReceiverController(IReceiverService receiverService)
        {
            _receiverService = receiverService;
        }

        public IActionResult Index(ReceiverIndexViewModel viewModel = null)
        {
            var resultViewModel = _receiverService.GetIndexViewModel(viewModel);
            return View(resultViewModel);
        }

        [HttpPost]
        public IActionResult UpdateMszAndCategories(string returnUrl, IFormFile xml)
        {
            
            var updated = true;
            try
            {
                XDocument xmlDoc = XDocument.Load(xml.OpenReadStream());
                updated = _receiverService.UpdateMszAndCategories(xmlDoc);
            }
            catch (XmlException)
            {
                updated = false;
            }
            if (!updated)
            {
                TempData["Error"] = "Некорректный формат файла. Ожидается xml-файл по форме 10.0.5.I";
            } else
            {
                TempData["Success"] = "МСЗ и категории успешно загружены";
            }
            return RedirectPermanent(returnUrl);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
