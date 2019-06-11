using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Tiktack.Common.DataAccessLayer.Repositories
{
    public interface IRepository<TDomain>
        where TDomain : class
    {
        IQueryable<TDomain> GetAll(Expression<Func<TDomain, bool>> filter = null,
            Func<IQueryable<TDomain>, IOrderedQueryable<TDomain>> orderBy = null,
            string includeProperties = "");

        Task<TDomain> GetById(int id);

        Task<TDomain> Add(TDomain entity);

        Task AddRange(IEnumerable<TDomain> entities);

        Task<TDomain> Update(TDomain entity);

        Task<TDomain> Remove(int id);
    }
}