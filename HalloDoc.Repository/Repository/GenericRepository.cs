using HalloDoc.Entities.DataModels;
using HalloDoc.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace HalloDoc.Repository.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private DbSet<T> _genericContext;
        private static readonly Dictionary<string, object> _tempData = new Dictionary<string, object>();
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _genericContext = _context.Set<T>();
        }
        
        public T? getFirstOrDefault(Expression<Func<T, bool>> filter) => _genericContext.FirstOrDefault(filter);
        public async Task AddAsync(T entity)
        {
            try
            {
                _context.Add(entity);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }

        public async Task AddAsyncss(T entity)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    context.AddAsync(entity);
                   await  context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {

            }
        }
        

        public async Task UpdateAsync(T entity)
        {
            try
            {

            _context.Update(entity);
            _context.SaveChanges();
            }
            catch(Exception ex)
            {

            }

        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await _context.Set<T>().FindAsync(id);
        }


        public T GetById(object id)
        {
            return _context.Set<T>().Find(id);
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

        public void Remove(T entity)
        {
            try
            {
                _context.Remove(entity);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {

            }
        }

        public string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }


        public string DecodeFrom64(string encodedData)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encodedData);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }


        public dynamic GetAllWithPagination(Expression<Func<T, object>> select, Expression<Func<T, bool>> where, int PageIndex, int PageSize, Expression<Func<T, object>> orderBy, bool IsAcc)
        {
            if (IsAcc)
                return _genericContext.Where(where).OrderBy(orderBy).Skip((PageIndex - 1) * PageSize).Take(PageSize).Select(select).ToList();

            return _genericContext.Where(where).OrderByDescending(orderBy).Skip((PageIndex - 1) * PageSize).Take(PageSize).Select(select).ToList();
        }

        public int GetTotalCount(Expression<Func<T, bool>> where)
        {
            return _genericContext.Count(where);
        }

        public dynamic GetAllData(Expression<Func<T, object>> select, Expression<Func<T, bool>> where)
        {
            return _genericContext.Where(where).Select(select).ToList();
        }

        public T? IncludeEntity(Expression<Func<T, bool>> expression, Expression<Func<T, object>> Include)
        {
            return _genericContext.Where(expression).Include(Include).FirstOrDefault();
        }
        public int GetCount(Expression<Func<T, object>> select, Expression<Func<T, bool>> where)
        {
            try
            {
                return _genericContext.Where(where).Select(select).Count();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }


}
