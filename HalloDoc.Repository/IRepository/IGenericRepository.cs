using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        public T? getFirstOrDefault(Expression<Func<T, bool>> filter);
        IQueryable<T> GetAll();
        Task AddAsync(T entity);
        Task AddAsyncss(T entity);

        // void AddAsyncs(T entity);
        Task UpdateAsync(T entity);
        //  void UpdateAsyncs(T entity);
        Task<T> GetByIdAsync(object id);
        void SetTempData(string key, object value);
        T GetTempData<T>(string key);
        string EncodePasswordToBase64(string password);
        string DecodeFrom64(string encodedData);
        public dynamic GetAllWithPagination(Expression<Func<T, object>> select, Expression<Func<T, bool>> where, int PageIndex, int PageSize, Expression<Func<T, object>> orderBy, bool IsAcc);
        public int GetTotalCount(Expression<Func<T, bool>> where);
        T GetById(object id);
        void Remove(T entity);
        dynamic GetAllData(Expression<Func<T, object>> select, Expression<Func<T, bool>> where);
        T? IncludeEntity(Expression<Func<T, bool>> expression, Expression<Func<T, object>> Include);
        int GetCount(Expression<Func<T, object>> select, Expression<Func<T, bool>> where);
        public List<Tentity> GetList<Tentity>(Expression<Func<T, Tentity>> select, Expression<Func<T, bool>> where) where Tentity : class;
    }
}
