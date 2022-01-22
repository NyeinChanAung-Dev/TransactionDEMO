using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionDEMO.Infrastructure.Repository
{
    public interface IRepository<T>
    {
        Task<T> InsertTransaction(T entity);
        Task<List<T>> InsertManyTransaction(List<T> entities);
        IQueryable<T> GetAll();
    }
}
