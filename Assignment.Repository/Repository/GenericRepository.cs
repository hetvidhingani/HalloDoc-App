using Assignment.Entities.DataModels;
using Assignment.Repository.IRepository;
using Assignment.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Repository.Repository
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
        public void Add(T entity)
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

        public void Update(T entity)
        {
            try
            {
                _context.Update(entity);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

            }

        }

        public T GetById(object id)
        {
            return _context.Set<T>().Find(id);
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

    }


}
