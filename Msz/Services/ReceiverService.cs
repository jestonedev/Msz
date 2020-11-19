using Msz.DatabaseContext;
using Msz.Options;
using Msz.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Msz.Models;
using System.Xml.Linq;
using System.Xml;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Msz.Services
{
    public class ReceiverService: IReceiverService
    {
        private readonly IMszDbContext _dbContext;
        private readonly IAclService _aclService;

        public ReceiverService(IMszDbContext dbContext, IAclService aclService)
        {
            _dbContext = dbContext;
            _aclService = aclService;
        }

        public ReceiverViewModel GetEmptyViewModel()
        {
            var user = _aclService.GetUser();
            var allowedMszs = new List<int>();
            if (user != null) {
                allowedMszs = _aclService.GetAllowedMszs(user);
            }
            return new ReceiverViewModel
            {
                Mszs = _dbContext.Mszs.Where(r => r.NextRevisionId == null && allowedMszs.Contains(r.Id)).OrderBy(r => r.Name).ToList(),
                Categories = _dbContext.Categories.Include(r => r.Msz).Where(r => r.Msz.NextRevisionId == null && allowedMszs.Contains(r.Msz.Id))
                    .OrderBy(r => r.Name).ToList(),
                Genders = _dbContext.Genders.ToList(),
                AssigmentForms = _dbContext.AssigmentForms.ToList(),
                Receiver = new Receiver {
                    Uuid = Guid.NewGuid().ToString(),
                    CreatedDate = DateTime.Now,
                    Creator = _aclService.GetLogin(),
                    ReasonPersons = new List<ReasonPerson>()
                }
            };
        }

        public ReceiverViewModel GetViewModel(int receiverId)
        {
            var receiver = _dbContext.Receivers
                    .Include(r => r.ReasonPersons).FirstOrDefault(r => r.Id == receiverId) ?? new Receiver
                    {
                        Uuid = Guid.NewGuid().ToString(),
                        CreatedDate = DateTime.Now,
                        Creator = _aclService.GetLogin(),
                        ReasonPersons = new List<ReasonPerson>()
                    };
            var canUpdate = _aclService.CanUpdate(receiver);

            var user = _aclService.GetUser();
            var allowedMszs = new List<int>();
            if (user != null)
            {
                allowedMszs = _aclService.GetAllowedMszs(user);
            }

            return new ReceiverViewModel
            {
                Mszs = _dbContext.Mszs.Where(r => r.NextRevisionId == null && (!canUpdate || allowedMszs.Contains(r.Id))).OrderBy(r => r.Name).ToList(),
                Categories = _dbContext.Categories.Include(r => r.Msz)
                    .Where(r => r.Msz.NextRevisionId == null && (!canUpdate || allowedMszs.Contains(r.Msz.Id)))
                    .OrderBy(r => r.Name).ToList(),
                Genders = _dbContext.Genders.ToList(),
                AssigmentForms = _dbContext.AssigmentForms.ToList(),
                Receiver = _dbContext.Receivers
                    .Include(r => r.ReasonPersons).FirstOrDefault(r => r.Id == receiverId) ?? new Receiver
                {
                    Uuid = Guid.NewGuid().ToString(),
                    CreatedDate = DateTime.Now,
                    Creator = _aclService.GetLogin(),
                    ReasonPersons = new List<ReasonPerson>()
                }
            };
        }

        public ReceiverIndexViewModel GetIndexViewModel(ReceiverIndexViewModel viewModel = null)
        {
            var resultViewModel = new ReceiverIndexViewModel
            {
                Mszs = _dbContext.Mszs.Where(r => r.NextRevisionId == null && !r.Inactive).OrderBy(r => r.Name).ToList(),
                Categories = _dbContext.Categories.Include(r => r.Msz).Where(r => r.Msz.NextRevisionId == null).OrderBy(r => r.Name).ToList(),
                FilterOptions = viewModel?.FilterOptions ?? new ReceiverFilterOptions(),
                PageOptions = viewModel?.PageOptions ?? new PageOptions()
            };
            var receivers = GetReceivers();
            var filteredReceivers = FilterReceivers(resultViewModel.FilterOptions,receivers);
            var receiversCount = filteredReceivers.Count();
            resultViewModel.PageOptions.PageCount = (int)Math.Ceiling(receiversCount / (decimal)resultViewModel.PageOptions.PageSize);
            resultViewModel.PageOptions.PageIndex = Math.Min(Math.Max(resultViewModel.PageOptions.PageCount - 1, 0), resultViewModel.PageOptions.PageIndex);
            resultViewModel.Receivers = filteredReceivers.OrderByDescending(r => r.CreatedDate)
                .Skip(resultViewModel.PageOptions.PageSize * resultViewModel.PageOptions.PageIndex)
                .Take(resultViewModel.PageOptions.PageSize).ToList();
            return resultViewModel;
        }

        private IQueryable<Receiver> GetReceivers()
        {
            return _dbContext.Receivers.Include(r => r.Msz).Include(r => r.Category).Include(r => r.ReasonPersons).Where(r => !r.IsDeleted);
        }

        private IQueryable<Receiver> FilterReceivers(ReceiverFilterOptions filterOptions, IQueryable<Receiver> receivers)
        {
            if (!string.IsNullOrEmpty(filterOptions.Surname))
            {
                if (filterOptions.IncludeReasonPersonsInFiltering)
                {
                    receivers = receivers.Where(r => r.Surname.Contains(filterOptions.Surname) || 
                        r.ReasonPersons.Any(rp => rp.Surname.Contains(filterOptions.Surname)));
                } else
                {
                    receivers = receivers.Where(r => r.Surname.Contains(filterOptions.Surname));
                }
            }
            if (!string.IsNullOrEmpty(filterOptions.Name))
            {
                if (filterOptions.IncludeReasonPersonsInFiltering)
                {
                    receivers = receivers.Where(r => r.Name.Contains(filterOptions.Name) ||
                        r.ReasonPersons.Any(rp => rp.Name.Contains(filterOptions.Name)));
                }
                else
                {
                    receivers = receivers.Where(r => r.Name.Contains(filterOptions.Name));
                }
            }
            if (!string.IsNullOrEmpty(filterOptions.Patronymic))
            {
                if (filterOptions.IncludeReasonPersonsInFiltering)
                {
                    receivers = receivers.Where(r => r.Patronymic.Contains(filterOptions.Patronymic) ||
                        r.ReasonPersons.Any(rp => rp.Patronymic.Contains(filterOptions.Patronymic)));
                }
                else
                {
                    receivers = receivers.Where(r => r.Patronymic.Contains(filterOptions.Patronymic));
                }
            }
            if (!string.IsNullOrEmpty(filterOptions.Snils))
            {
                if (filterOptions.IncludeReasonPersonsInFiltering)
                {
                    receivers = receivers.Where(r => r.Snils == filterOptions.Snils ||
                        r.ReasonPersons.Any(rp => rp.Snils == filterOptions.Snils));
                }
                else
                {
                    receivers = receivers.Where(r => r.Snils == filterOptions.Snils);
                }
            }
            if (!string.IsNullOrEmpty(filterOptions.Address))
            {
                receivers = receivers.Where(r => r.Address != null && r.Address.Contains(filterOptions.Address));
            }
            if (filterOptions.MszId != null)
            {
                receivers = receivers.Where(r => r.MszId == filterOptions.MszId);
            } else
            {
                var allowedMszs = new List<int>();
                var user = _aclService.GetUser();
                if (user != null)
                {
                    allowedMszs = _aclService.GetAllowedMszs(user);
                }
                receivers = receivers.Where(r => allowedMszs.Contains(r.MszId));
            }
            if (filterOptions.CategoryId != null)
            {
                receivers = receivers.Where(r => r.CategoryId == filterOptions.CategoryId);
            }
            if (filterOptions.DecisionDate != null)
            {
                receivers = receivers.Where(r => r.DecisionDate == filterOptions.DecisionDate);
            }
            if (filterOptions.DecisionNumber != null)
            {
                receivers = receivers.Where(r => r.DecisionNumber != null && r.DecisionNumber.Contains(filterOptions.DecisionNumber));
            }
            if (filterOptions.StartDate != null)
            {
                receivers = receivers.Where(r => r.StartDate == filterOptions.StartDate);
            }
            if (filterOptions.EndDate != null)
            {
                receivers = receivers.Where(r => r.EndDate != null && r.EndDate == filterOptions.EndDate);
            }
            if (filterOptions.CreateByMe)
            {
                receivers = receivers.Where(r => r.Creator.Contains(_aclService.GetLogin()));
            }
            if (filterOptions.ModifyDate != null)
            {
                receivers = receivers.Where(r => r.CreatedDate.Date == filterOptions.ModifyDate);
                var ids = receivers.Select(r => r.Id).ToList();
                receivers = receivers.Where(r => !ids.Any(ri => ri == r.NextRevisionId));
            } else
            {
                receivers = receivers.Where(r => r.NextRevisionId == null);
            }
            return receivers;
        }

        public XDocument CreateRecieversXml(ReceiverFilterOptions filterOptions)
        {
            XNamespace xmlns = "urn://egisso-ru/types/package-RAF/1.0.7";
            XNamespace xmlns2 = "urn://egisso-ru/types/assignment-fact/1.0.6";
            XNamespace xmlns3 = "urn://egisso-ru/types/prsn-info/1.0.3";
            XNamespace xmlns4 = "urn://x-artefacts-smev-gov-ru/supplementary/commons/1.0.1";
            XNamespace xmlns5 = "urn://egisso-ru/types/basic/1.0.4";
            XNamespace xmlns6 = "urn://egisso-ru/msg/10.06.S/1.0.6";
            XNamespace xmlns7 = "urn://egisso-ru/types/package-RAF/1.0.6";

            var elements = new XElement(xmlns + "elements");

            var doc = new XDocument(
                new XDeclaration("1.0", "UTF-8", "yes"),
                new XElement(xmlns6+"data",
                    new XAttribute("xmlns", "urn://egisso-ru/types/package-RAF/1.0.7"),
                    new XAttribute(XNamespace.Xmlns + "ns2", xmlns2.NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "ns3", xmlns3.NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "ns4", xmlns4.NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "ns5", xmlns5.NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "ns6", xmlns6.NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "ns7", xmlns7.NamespaceName),
                    new XElement(xmlns+"package", 
                        new XElement(xmlns + "packageId", Guid.NewGuid().ToString()),
                        elements
                    )
                )
            );

            var receivers = GetReceivers();
            var filteredReceivers = FilterReceivers(filterOptions, receivers);

            foreach(var receiver in filteredReceivers)
            {
                XElement reasonPersons = null;
                if (receiver.ReasonPersons != null && receiver.ReasonPersons.Any())
                {
                    reasonPersons = new XElement(xmlns2 + "reasonPersons");
                    foreach (var person in receiver.ReasonPersons)
                    {
                        reasonPersons.Add(
                            new XElement(xmlns3+ "prsnInfo",
                                new XElement(xmlns3 + "SNILS", person.Snils.Replace("-", "")),
                                new XElement(xmlns4 + "FamilyName", person.Surname),
                                new XElement(xmlns4 + "FirstName", person.Name),
                                person.Patronymic != null ? new XElement(xmlns4 + "Patronymic", person.Patronymic) : null,
                                new XElement(xmlns3 + "Gender", person.GenderId == 1 ? "Male" : "Female"),
                                new XElement(xmlns3 + "BirthDate", person.BirthDate.ToString("yyyy-MM-dd") + "+03:00")
                            )
                        );
                    }
                }

                elements.Add(new XElement(xmlns + "fact",
                    new XElement(xmlns2 + "oszCode", "3570.000001"),
                    new XElement(xmlns2 + "mszReceiver", 
                        new XElement(xmlns3+"SNILS", receiver.Snils.Replace("-", "")),
                        new XElement(xmlns4 + "FamilyName", receiver.Surname),
                        new XElement(xmlns4 + "FirstName", receiver.Name),
                        receiver.Patronymic != null ? new XElement(xmlns4 + "Patronymic", receiver.Patronymic) : null,
                        new XElement(xmlns3 + "Gender", receiver.GenderId == 1 ? "Male" : "Female"),
                        new XElement(xmlns3 + "BirthDate", receiver.BirthDate.ToString("yyyy-MM-dd") + "+03:00")
                    ),
                    reasonPersons,
                    new XElement(xmlns2 + "lmszId", receiver.Msz.Guid),
                    new XElement(xmlns2 + "categoryId", receiver.Category.Guid),
                    new XElement(xmlns2 + "decisionDate", receiver.DecisionDate.ToString("yyyy-MM-dd")+"+03:00"),
                    new XElement(xmlns2 + "dateStart", receiver.StartDate.ToString("yyyy-MM-dd") + "+03:00"),
                    receiver.EndDate != null ? new XElement(xmlns2 + "dateFinish", receiver.EndDate.Value.ToString("yyyy-MM-dd") + "+03:00") : null,
                    new XElement(xmlns2 + "needsCriteria", 
                        new XElement(xmlns2 + "usingSign", "false")
                    ),
                    new XElement(xmlns2 + "assignmentInfo", 
                        receiver.AssigmentFormId == 1 ? 
                        new XElement(xmlns2+ "monetaryForm",
                            new XElement(xmlns2+"amount", receiver.Amount.ToString(new CultureInfo("ru-RU")))) : 
                        new XElement(xmlns2 + "exemptionForm",
                            new XElement(xmlns2 + "amount", receiver.Amount.ToString(new CultureInfo("ru-RU")).Replace(",00", "")),
                            new XElement(xmlns2 + "measuryCode", "05"),
                            new XElement(xmlns2 + "monetization", "false"),
                            new XElement(xmlns2 + "equivalentAmount",
                                receiver.EquivalentAmount != null ?
                                receiver.EquivalentAmount.Value.ToString(new CultureInfo("ru-RU")).Replace(",00", "").Replace(",", ".") : "0")
                        )
                    
                    ),
                    new XElement(xmlns + "uuid", Guid.NewGuid().ToString())
                    )
                );
            }

            return doc;
        }

        public bool UpdateMszAndCategories(XDocument xml)
        {
            var mszs = ParseMszs(xml);
            var login = _aclService.GetLogin();
            foreach (var msz in mszs)
            {
                msz.Creator = login;
                var prevMsz = GetPreviousMsz(msz);
                if (prevMsz != null)
                    msz.PreviousRevisionId = prevMsz.Id;
                if (prevMsz != null && prevMsz.CreatedDate >= msz.CreatedDate)
                {
                    continue;
                }
                InsertMsz(msz);
                _dbContext.SaveChanges();
                if (prevMsz != null)
                {
                    prevMsz.NextRevisionId = msz.Id;
                    _dbContext.Mszs.Update(prevMsz);
                    _dbContext.SaveChanges();
                }
            }
            if (mszs == null || !mszs.Any())
                return false;
            _dbContext.ActualizateReceiversMszIds();
            return true;
        }

        private Models.Msz GetPreviousMsz(Models.Msz msz)
        {
            var prevMsz = _dbContext.Mszs.FirstOrDefault(r => r.Guid == msz.PreviousGuid && r.NextRevisionId == null);
            if (prevMsz == null)
            {
                return _dbContext.Mszs.FirstOrDefault(r => r.Name == msz.Name && r.NextRevisionId == null);
            }
            return prevMsz;
        }

        private void InsertMsz(Models.Msz msz)
        {
            _dbContext.Mszs.Add(msz);
        }

        public List<Models.Msz> ParseMszs(XDocument xml)
        {
            var xmlns = "urn://egisso-ru/types/package-LMSZ/1.0.6";
            var xmlns2 = "urn://egisso-ru/types/local-MSZ/1.0.5";
            var root = xml.Root;
            var package = root.Element(XName.Get("package", xmlns));
            if (package == null)
            {
                xmlns = "urn://egisso-ru/types/package-LMSZ/1.0.8";
                xmlns2 = "urn://egisso-ru/types/local-MSZ/1.0.7";
            }
            package = root.Element(XName.Get("package", xmlns));
            if (package == null)
            {
                return null;
            }
            var elements = package.Element(XName.Get("elements", xmlns));
            if (elements == null)
            {
                return null;
            }
            var localMSZs = elements.Elements();
            var mszs = new List<Models.Msz>();
            foreach (var localMSZ in localMSZs)
            {
                var msz = new Models.Msz { Categories = new List<Category>() };
                if (localMSZ.Name != XName.Get("localMSZ", xmlns))
                {
                    return null;
                }
                var mszGuid = localMSZ.Element(XName.Get("ID", xmlns2));
                if (mszGuid == null) return null;
                msz.Guid = mszGuid.Value;
                var mszCode = localMSZ.Element(XName.Get("code", xmlns2));
                if (mszCode == null) return null;
                var mszTitle = localMSZ.Element(XName.Get("title", xmlns2));
                if (mszTitle == null) return null;
                msz.Name = "("+mszCode.Value + ") " + mszTitle.Value;
                var classification = localMSZ.Element(XName.Get("classificationKMSZ", xmlns2));
                if (classification == null) return null;
                var localCategoriesRoot = classification.Element(XName.Get("localCategories", xmlns2));
                if (localCategoriesRoot == null) return null;
                var localCategories = localCategoriesRoot.Elements(XName.Get("localCategory", xmlns2));
                foreach(var locaCategory in localCategories)
                {
                    var category = new Category();
                    var guid = locaCategory.Element(XName.Get("ID", xmlns2));
                    if (guid == null) return null;
                    category.Guid = guid.Value;
                    var code = locaCategory.Element(XName.Get("code", xmlns2));
                    if (code == null) return null;
                    var title = locaCategory.Element(XName.Get("title", xmlns2));
                    if (title == null) return null;
                    category.Name = "("+code.Value + ") " + title.Value;
                    msz.Categories.Add(category);
                }
                var lastChanging = localMSZ.Element(XName.Get("lastChanging", xmlns));
                if (lastChanging == null) return null;
                msz.CreatedDate = DateTime.Parse(lastChanging.Value);

                var previousGuid = localMSZ.Element(XName.Get("previousID", xmlns));
                if (previousGuid != null)
                {
                    msz.PreviousGuid = previousGuid.Value;
                }
                mszs.Add(msz);
            }

            return mszs;
        }

        public void Insert(Receiver receiver)
        {
            receiver.CreatedDate = DateTime.Now;
            receiver.Creator = _aclService.GetLogin();
            _dbContext.Receivers.Add(receiver);
            _dbContext.SaveChanges();
        }

        public void Update(Receiver receiver)
        {
            var prevReceiver = _dbContext.Receivers.FirstOrDefault(r => r.Id == receiver.Id);
            if (prevReceiver != null)
            {
                receiver.PrevRevisionId = prevReceiver.Id;
            }
            receiver.Id = 0;
            receiver.Uuid = Guid.NewGuid().ToString();
            Insert(receiver);
            if (prevReceiver != null)
            {
                prevReceiver.NextRevisionId = receiver.Id;
                _dbContext.Receivers.Update(prevReceiver);
                _dbContext.SaveChanges();
            }
        }

        public void Delete(int receiverId)
        {
            var receiver = _dbContext.Receivers.FirstOrDefault(r => r.Id == receiverId);
            if (receiver != null)
            {
                receiver.IsDeleted = true;
                _dbContext.Receivers.Update(receiver);
                _dbContext.SaveChanges();
            }
        }
        public void Copy(Receiver receiver)
        {
            receiver.CreatedDate = DateTime.Now;
            receiver.Creator = _aclService.GetLogin();
            receiver.Id = 0;
            receiver.Uuid = Guid.NewGuid().ToString();
            Insert(receiver);
        }
    }
}
