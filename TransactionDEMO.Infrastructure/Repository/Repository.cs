using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionDEMO.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly TransactionDBContext _dbContext;

        public Repository(TransactionDBContext dbContex)
        {
            _dbContext = dbContex;
        }

        public async Task<T> InsertTransaction(T entity)
        {
            T newEntity = _dbContext.Set<T>().Add(entity).Entity;
            var result = await _dbContext.SaveChangesAsync();
            if (result > 0)
            {
                return newEntity;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<T>> InsertManyTransaction(IEnumerable<T> entities)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
            var result = await _dbContext.SaveChangesAsync();
            if (result > 0)
            {
                return entities;
            }
            else
            {
                return null;
            }
        }

        public IQueryable<T> GetAll()
        {
            return _dbContext.Set<T>().AsQueryable().AsNoTracking();
        }

    }
}
