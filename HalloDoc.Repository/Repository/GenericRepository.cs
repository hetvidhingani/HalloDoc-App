 using HalloDoc.Entities.DataModels;
using HalloDoc.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.Repository
{
    public class GenericRepository<T>:IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private static readonly Dictionary<string, object> _tempData = new Dictionary<string, object>();
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(T entity)
        {
                _context.Add(entity);
                await _context.SaveChangesAsync();
        }
      
        public async Task UpdateAsync(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();

        }
        public async Task<T> GetByIdAsync(object id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        public void SetTempData(string key, object value)
        {
            _tempData[key] = value;
        }

        public T GetTempData<T>(string key)
        {
            if (_tempData.ContainsKey(key))
            {
                return (T)_tempData[key];
            }
            else
            {
                return default(T);
            }
        }
        public IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }

    }
}
