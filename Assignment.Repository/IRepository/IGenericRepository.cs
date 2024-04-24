using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Repository.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        void Add(T entity);
        void Update(T entity);
        T GetById(object id);
        IQueryable<T> GetAll();
        void Remove(T entity);
        dynamic GetAllWithPagination(Expression<Func<T, object>> select, Expression<Func<T, bool>> where, int PageIndex, int PageSize, Expression<Func<T, object>> orderBy, bool IsAcc);
        int GetTotalCount(Expression<Func<T, bool>> where);
        dynamic GetAllData(Expression<Func<T, object>> select, Expression<Func<T, bool>> where);
        T? IncludeEntity(Expression<Func<T, bool>> expression, Expression<Func<T, object>> Include);
    }
}
