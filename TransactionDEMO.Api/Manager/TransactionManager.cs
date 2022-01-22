using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransactionDEMO.Api.Models;
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

        public async Task<List<Transaction>> InsertTransactions(List<Transaction> transactions)
        {
            List<Transaction> response = await _transactionRepo.InsertManyTransaction(transactions);
            return response;
        }

        public async Task<PagedListModel<ResponseTransaction>> GetAllTransactions(int page = 1, int pageSize = 10)
        {
            var finalResult = new PagedListModel<ResponseTransaction>();
            List<ResponseTransaction> response = await (from tranOrg in _transactionRepo.GetAll()
                                                        select new ResponseTransaction()
                                                        {
                                                            Id = tranOrg.TransactionId,
                                                            Payment = tranOrg.Amount.ToString() + " " + tranOrg.CurrencyCode ,
                                                            Status = tranOrg.Status
                                                        }).ToListAsync();

            if (response.Count() != 0)
            {
                #region paging
                var totalCount = response.Count();
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                var results = response.Skip(pageSize * (page - 1))
                                     .Take(pageSize);

                PagedListModel<ResponseTransaction> model = new PagedListModel<ResponseTransaction>()
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

        public async Task<List<ResponseTransaction>> GetTransactionsByCurrency(string currencyCode)
        {
            List<ResponseTransaction> response = await (from tranOrg in _transactionRepo.GetAll()
                                                        where tranOrg.CurrencyCode == currencyCode
                                                        select new ResponseTransaction()
                                                        {
                                                            Id = tranOrg.TransactionId,
                                                            Payment = tranOrg.Amount.ToString() + " " + tranOrg.CurrencyCode,
                                                            Status = tranOrg.Status
                                                        }).ToListAsync();
            return response;
        }

        public async Task<List<ResponseTransaction>> GetTransactionsByDate(DateTime startDate, DateTime endDate)
        {
            List<ResponseTransaction> response = await (from tranOrg in _transactionRepo.GetAll()
                                                        where tranOrg.TransactionDate >= startDate && tranOrg.TransactionDate <= endDate
                                                        select new ResponseTransaction()
                                                        {
                                                            Id = tranOrg.TransactionId,
                                                            Payment = tranOrg.Amount.ToString() + " " + tranOrg.CurrencyCode,
                                                            Status = tranOrg.Status
                                                        }).ToListAsync();
            return response;
        }

        public async Task<List<ResponseTransaction>> GetTransactionsByStatus(string status)
        {
            List<ResponseTransaction> response = await (from tranOrg in _transactionRepo.GetAll()
                                                        where tranOrg.Status == status
                                                        select new ResponseTransaction()
                                                        {
                                                            Id = tranOrg.TransactionId,
                                                            Payment = tranOrg.Amount.ToString() + " " + tranOrg.CurrencyCode,
                                                            Status = tranOrg.Status
                                                        }).ToListAsync();
            return response;
        }

    }
}
