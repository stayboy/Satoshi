using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Satoshi.Repository
{
    public abstract class DBRepositoryBase<T, TContext> where T : class where TContext : DbContext
    {
        protected private readonly TContext pcontext; 

        private readonly DbSet<T> _entity;
        public DBRepositoryBase(TContext _context)
        {
            pcontext = _context;
            _entity = pcontext.Set<T>();
        }

        public DbSet<T> Entity => pcontext.Set<T>();        
        public void Create(T entity) => pcontext.Set<T>().Add(entity);
        public void Delete(T entity) => pcontext.Set<T>().Remove(entity);
      
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, int? Top = 100, bool trackChanges = false) =>
            !trackChanges ? _entity.Where(expression).AsNoTracking() : _entity.Where(expression).Take((Top ?? 100));
        public void Update(T entity) => pcontext.Set<T>().Update(entity);

    }
}
