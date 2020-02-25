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
using System.Globalization;
using Msz.Helpers;
using Msz.Options;
using System.IO;
using System.Text;

namespace Msz.Controllers
{
    public class ReceiverController : Controller
    {
        private readonly IReceiverService _receiverService;
        private readonly IAclService _aclService;

        public ReceiverController(IReceiverService receiverService, IAclService aclService)
        {
            _receiverService = receiverService;
            _aclService = aclService;
        }

        public IActionResult Index(ReceiverIndexViewModel viewModel = null)
        {
            if (!_aclService.CanReadAny()) return RedirectToAction("Error", new { Message = "У вас нет прав доступа к этому приложению" });
            ViewData["aclService"] = _aclService;
            if (viewModel.FilterOptions != null)
            {
                viewModel.FilterOptions.StartDate = DateBinder.FromUrl("FilterOptions.StartDate");
                viewModel.FilterOptions.EndDate = DateBinder.FromUrl("FilterOptions.EndDate");
                viewModel.FilterOptions.DecisionDate = DateBinder.FromUrl("FilterOptions.DecisionDate");
                viewModel.FilterOptions.ModifyDate = DateBinder.FromUrl("FilterOptions.ModifyDate");
            }
            var resultViewModel = _receiverService.GetIndexViewModel(viewModel);
            return View(resultViewModel);
        }

        public IActionResult Create(string returnUrl)
        {
            if (!_aclService.CanInsertAny()) return RedirectToAction("Error", new { Message = "У вас нет прав на добавление получателей МСЗ" });
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
                if (!_aclService.CanInsert(viewModel.Receiver))
                {
                    return RedirectToAction("Error", new { Message = "У вас нет прав на добавление получателей МСЗ" });
                }
                _receiverService.Insert(viewModel.Receiver);
                return Redirect(returnUrl);
            }
        }

        public IActionResult Update(string returnUrl, int id)
        {
            if (!_aclService.CanReadAny()) return RedirectToAction("Error", new { Message = "У вас нет прав доступа к этому приложению" });
            ViewData["returnUrl"] = returnUrl;
            ViewData["aclService"] = _aclService;
            var viewModel = _receiverService.GetViewModel(id);
            return View(viewModel);
        }
        

        [HttpPost]
        public IActionResult Update(ReceiverViewModel viewModel, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                var newViewModel = _receiverService.GetEmptyViewModel();
                newViewModel.Receiver = viewModel.Receiver;
                if (newViewModel.Receiver.ReasonPersons == null)
                    newViewModel.Receiver.ReasonPersons = new List<ReasonPerson>();
                return View(newViewModel);
            }
            else
            {
                if (!_aclService.CanUpdate(viewModel.Receiver))
                {
                    return RedirectToAction("Error", new { Message = "У вас нет прав на изменение данного получателя МСЗ" });
                }
                _receiverService.Update(viewModel.Receiver);
                return Redirect(returnUrl);
            }
        }

        [HttpPost]
        public IActionResult Delete(string returnUrl, int id)
        {
            if (!_aclService.CanDelete(_receiverService.GetViewModel(id).Receiver))
            {
                return RedirectToAction("Error", new { Message = "У вас нет прав на удаление данного получателя МСЗ" });
            }
            _receiverService.Delete(id);
            return Redirect(returnUrl);
        }

        [HttpPost]
        public IActionResult UpdateMszAndCategories(string returnUrl, IFormFile xml)
        {
            if (!_aclService.CanReadAny()) return RedirectToAction("Error", new { Message = "У вас нет прав доступа к этому приложению" });

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

        public IActionResult DownloadFilteredRecievers(ReceiverFilterOptions filterOptions)
        {
            if (!_aclService.CanReadAny()) return RedirectToAction("Error", new { Message = "У вас нет прав доступа к этому приложению" });
            filterOptions.StartDate = DateBinder.FromUrl("FilterOptions.StartDate");
            filterOptions.EndDate = DateBinder.FromUrl("FilterOptions.EndDate");
            filterOptions.DecisionDate = DateBinder.FromUrl("FilterOptions.DecisionDate");
            filterOptions.ModifyDate = DateBinder.FromUrl("FilterOptions.ModifyDate");
            var user = _aclService.GetUser();
            var egissoId = user.EgissoId ?? "0000000000";
            XDocument xml = _receiverService.CreateRecieversXml(filterOptions);
            var xmlStream = new MemoryStream();
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Encoding = new UTF8Encoding(false)
            };
            using (XmlWriter w = XmlWriter.Create(xmlStream, settings))
            {
                xml.Save(w);
            }
            xmlStream.Seek(0, SeekOrigin.Begin);
            return File(xmlStream, "text/xml", string.Format("{0}-10.06.S-{1}.xml", egissoId, DateTime.Now.AddHours(-5).ToString("yyyy-MM-ddTHH-mm-ss.mmm")));
        }

        public IActionResult GetEmptyReasonPerson()
        {
            var emptyViewModel = _receiverService.GetEmptyViewModel();
            emptyViewModel.Receiver.ReasonPersons.Add(new ReasonPerson { });
            ViewData["index"] = 0;
            return View("ReasonPerson", emptyViewModel);
        }
        
        public IActionResult Error(string message)
        {
            return View(new ErrorViewModel { Message = message });
        }
        public IActionResult Copy(string returnUrl, int id)
        {
            if (!_aclService.CanReadAny()) return RedirectToAction("Error", new { Message = "У вас нет прав доступа к этому приложению" });
            ViewData["returnUrl"] = returnUrl;
            ViewData["aclService"] = _aclService;
            var viewModel = _receiverService.GetViewModel(id);
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Copy(ReceiverViewModel viewModel, string returnUrl)
        {
            if (!_aclService.CanUpdate(viewModel.Receiver))
            {
                return RedirectToAction("Error", new { Message = "У вас нет прав на изменение данного получателя МСЗ" });
            }
            _receiverService.Copy(viewModel.Receiver);
            return Redirect(returnUrl);
        }
    }
}
