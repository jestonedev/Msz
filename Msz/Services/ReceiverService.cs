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
            return new ReceiverViewModel
            {
                Mszs = _dbContext.Mszs.Where(r => r.NextRevisionId == null).OrderBy(r => r.Name).ToList(),
                Categories = _dbContext.Categories.Include(r => r.Msz).Where(r => r.Msz.NextRevisionId == null).OrderBy(r => r.Name).ToList(),
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
            return new ReceiverViewModel
            {
                Mszs = _dbContext.Mszs.Where(r => r.NextRevisionId == null).OrderBy(r => r.Name).ToList(),
                Categories = _dbContext.Categories.Include(r => r.Msz).Where(r => r.Msz.NextRevisionId == null).OrderBy(r => r.Name).ToList(),
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
                Mszs = _dbContext.Mszs.Where(r => r.NextRevisionId == null).OrderBy(r => r.Name).ToList(),
                Categories = _dbContext.Categories.Include(r => r.Msz).Where(r => r.Msz.NextRevisionId == null).OrderBy(r => r.Name).ToList(),
                FilterOptions = viewModel?.FilterOptions ?? new ReceiverFilterOptions(),
                PageOptions = viewModel?.PageOptions ?? new PageOptions()
            };
            var receivers = _dbContext.Receivers.Include(r => r.Msz).Include(r => r.Category).Where(r => r.NextRevisionId == null && !r.IsDeleted);
            var filteredReceivers = FilterReceivers(resultViewModel.FilterOptions,receivers);
            var receiversCount = filteredReceivers.Count();
            resultViewModel.PageOptions.PageCount = (int)Math.Ceiling(receiversCount / (decimal)resultViewModel.PageOptions.PageSize);
            resultViewModel.PageOptions.PageIndex = Math.Min(Math.Max(resultViewModel.PageOptions.PageCount - 1, 0), resultViewModel.PageOptions.PageIndex);
            resultViewModel.Receivers = filteredReceivers.Skip(resultViewModel.PageOptions.PageSize * resultViewModel.PageOptions.PageIndex)
                .Take(resultViewModel.PageOptions.PageSize).ToList();
            return resultViewModel;
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
            if (filterOptions.ModifyDate != null)
            {
                receivers = receivers.Where(r => r.CreatedDate.Date == filterOptions.ModifyDate);
            }
            return receivers;
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
    }
}
