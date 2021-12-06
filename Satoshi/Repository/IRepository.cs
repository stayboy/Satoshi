using System.Linq.Expressions;

namespace Satoshi.Repository;
public interface IRepository<T>
{
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, int? Top = 100, bool trackChanges = false);
    void CreateEntity(T entity);
    void UpdateEntity(T entity);
    void DeleteEntity(T entity);

}

