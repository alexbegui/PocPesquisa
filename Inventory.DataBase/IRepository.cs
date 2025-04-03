using System.Linq.Expressions;

namespace CensusFieldSurvey.DataBase
{
    public interface IRepository<T>
    {
        Task<T?> GetById(int id);

        Task<List<T>> GetAll();

        Task Add(T obj);

        Task Update(T obj);

        Task Remove(int id);

        Task<List<T>> FindByCondition(Expression<Func<T, bool>> expression);

        Task<List<T>> FindByConditionAsync(Expression<Func<T, bool>> expression, params string[] propertiesToInclude);


    }
}