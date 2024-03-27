using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        Task AddAsync(T entity);

       // void AddAsyncs(T entity);
        Task UpdateAsync(T entity);
      //  void UpdateAsyncs(T entity);
        Task<T> GetByIdAsync(object id);
        void SetTempData(string key, object value);
        T GetTempData<T>(string key);
        string EncodePasswordToBase64(string password);
        string DecodeFrom64(string encodedData);

        T GetById(object id);
    }
}
