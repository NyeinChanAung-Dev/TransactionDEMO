using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransactionDEMO.Api.Utils;
using TransactionDEMO.Domain.Models;
using TransactionDEMO.Infrastructure;
using TransactionDEMO.Infrastructure.Repository;

namespace TransactionDEMO.Api.Manager
{
    public class TransactionManager
    {
        TransactionDBContext _dbContext;
        IRepository<Transaction> _transactionRepo;

        public TransactionManager()
        {
            _dbContext = new TransactionDBContext();
            _transactionRepo = new Repository<Transaction>(_dbContext);
        }

        public async Task<Transaction> InsertTransaction(Transaction transaction)
        {
            transaction.Id = Guid.NewGuid();
            Transaction response = await _transactionRepo.InsertTransaction(transaction);
            return response;
        }

        public async Task<IEnumerable<Transaction>> InsertTransactions(IEnumerable<Transaction> transactions)
        {
            IEnumerable<Transaction> response = await _transactionRepo.InsertManyTransaction(transactions);
            return response;
        }

        public async Task<PagedListModel<Transaction>> GetAllTransactions(int page = 1, int pageSize = 10)
        {
            var finalResult = new PagedListModel<Transaction>();
            List<Transaction> response = await _transactionRepo.GetAll().ToListAsync();
            if (response.Count() != 0)
            {
                #region paging
                var totalCount = response.Count();
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                var results = response.Skip(pageSize * (page - 1))
                                     .Take(pageSize);

                PagedListModel<Transaction> model = new PagedListModel<Transaction>()
                {
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    prevLink = "",
                    nextLink = "",
                    Results = results.ToList(),
                };
                #endregion

                finalResult = model;
            }
            else
            {
                finalResult.Results = null;
                finalResult.TotalPages = 0;
                finalResult.TotalCount = 0;
            }

            return finalResult;
        }

        public async Task<List<Transaction>> GetTransactionsByCurrency(string currencyCode)
        {
            List<Transaction> response = await _transactionRepo.GetAll().Where(t => t.CurrencyCode == currencyCode).ToListAsync();
            return response;
        }

        public async Task<List<Transaction>> GetTransactionsByDate(DateTime startDate, DateTime endDate)
        {
            List<Transaction> response = await _transactionRepo.GetAll().Where(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate).ToListAsync();
            return response;
        }

        public async Task<List<Transaction>> GetTransactionsByStatus(string status)
        {
            List<Transaction> response = await _transactionRepo.GetAll().Where(t => t.Status == status).ToListAsync();
            return response;
        }

    }
}
