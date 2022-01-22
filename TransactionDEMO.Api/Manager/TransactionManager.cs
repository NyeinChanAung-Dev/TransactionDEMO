using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
