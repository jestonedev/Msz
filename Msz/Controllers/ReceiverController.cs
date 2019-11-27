﻿using System;
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

        public IActionResult Create(string returnUrl)
        {
            ViewData["returnUrl"] = returnUrl;
            return View(_receiverService.GetEmptyViewModel());
        }

        [HttpPost]
        public IActionResult Create(ReceiverViewModel viewModel, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                var newViewModel = _receiverService.GetEmptyViewModel();
                newViewModel.Receiver = viewModel.Receiver;
                if (newViewModel.Receiver.ReasonPersons == null)
                    newViewModel.Receiver.ReasonPersons = new List<ReasonPerson>();
                return View(newViewModel);
            } else
            {
                return RedirectPermanent(returnUrl);
            }
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

        public IActionResult GetEmptyReasonPerson()
        {
            var emptyViewModel = _receiverService.GetEmptyViewModel();
            emptyViewModel.Receiver.ReasonPersons.Add(new ReasonPerson { });
            ViewData["index"] = 0;
            return View("ReasonPerson", emptyViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
