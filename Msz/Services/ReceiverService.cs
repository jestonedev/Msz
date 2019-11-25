using Msz.DatabaseContext;
using Msz.Options;
using Msz.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Msz.Models;

namespace Msz.Services
{
    public class ReceiverService: IReceiverService
    {
        private readonly IMszDbContext _dbContext;

        public ReceiverService(IMszDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ReceiverIndexViewModel GetIndexViewModel(ReceiverIndexViewModel viewModel = null)
        {
            var resultViewModel = new ReceiverIndexViewModel
            {
                Mszs = _dbContext.Mszs.ToList(),
                Categories = _dbContext.Categories.ToList(),
                FilterOptions = viewModel?.FilterOptions ?? new ReceiverFilterOptions(),
                PageOptions = viewModel?.PageOptions ?? new PageOptions()
            };
            var receivers = _dbContext.Receivers.Include(r => r.Msz).Include(r => r.Category).Where(r => r.NextRevisionId == null);
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
            return receivers;
        }
    }
}
