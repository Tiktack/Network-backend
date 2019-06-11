using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Tiktack.Common.DataAccessLayer.Repositories
{
    public class Repository<TDomain> : IRepository<TDomain> where TDomain : class
    {
        private readonly DbContext _context;
        private readonly DbSet<TDomain> _dbSet;

        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TDomain>();
        }

        public async Task<TDomain> GetById(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<TDomain> Add(TDomain entity)
        {
            var entry = await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task AddRange(IEnumerable<TDomain> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public async Task<TDomain> Update(TDomain entity)
        {
            var entry = _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<TDomain> Remove(int id)
        {
            var entity = await GetById(id);
            var entry = _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }

        public IQueryable<TDomain> GetAll(
            Expression<Func<TDomain, bool>> filter = null,
            Func<IQueryable<TDomain>, IOrderedQueryable<TDomain>> orderBy = null,
            string includeProperties = "")
        {
            var query = _dbSet.AsNoTracking();
            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = includeProperties
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return orderBy != null ? orderBy(query) : query;
        }
    }
}
