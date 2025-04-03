using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CensusFieldSurvey.DataBase
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T?> GetById(int id)
        {
            try
            {
                var value = await _dbSet.FindAsync(id);
                if (value != null)
                {
                    return value;
                }
                else
                {
                    return null;
                }
            }
            catch (DbUpdateException e)
            {
                throw new Exception(" Falha ao realizar a consulta no banco. " + e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(" Falha ao realizar a consulta no banco. " + e.Message);
            }
        }

        public async Task<List<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }


        public async Task Add(T obj)
        {
            try
            {
                await _dbSet.AddAsync(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new Exception(" Falha ao adicionar o item no banco. " + e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(" Falha ao adicionar o item no banco. " + e.Message);
            }
        }

        public async Task Update(T obj)
        {
            try
            {
                _dbSet.Update(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new Exception(" Falha ao atualizar o item no banco. " + e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(" Falha ao atualizar o item no banco. " + e.Message);
            }
        }

        public async Task Remove(int id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity != null)
                {
                    _dbSet.Remove(entity);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException e)
            {
                throw new Exception(" Falha ao remover o item no banco. " + e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(" Falha ao remover o item no banco. " + e.Message);
            }
        }

        public async Task<List<T>> FindByCondition(Expression<Func<T, bool>> expression)
        {
            try
            {
                return await _dbSet.Where(expression).ToListAsync();
            }
            catch (DbUpdateException e)
            {
                throw new Exception("Falha ao realizar a consulta no banco. " + e.Message);
            }
            catch (Exception e)
            {
                throw new Exception("Falha ao realizar a consulta no banco. " + e.Message);
            }
        }

        public async Task<List<T>> FindByConditionAsync(Expression<Func<T, bool>> expression, params string[] propertiesToInclude)
        {
            try
            {
                IQueryable<T> query = _dbSet.Where(expression);

                if (propertiesToInclude != null)
                {
                    query = query.IncludeProperties(propertiesToInclude);
                }

                return await query.ToListAsync();
            }
            catch (DbUpdateException e)
            {
                throw new Exception("Falha ao realizar a consulta no banco. " + e.Message);
            }
            catch (Exception e)
            {
                throw new Exception("Falha ao realizar a consulta no banco. " + e.Message);
            }
        }


    }

    public static class IQueryableExtensions
    {
        public static IQueryable<T> IncludeProperties<T>(this IQueryable<T> query, params string[] propertiesToInclude) where T : class
        {
            foreach (var property in propertiesToInclude)
            {
                query = query.Include(property);
            }
            return query;
        }
    }


}