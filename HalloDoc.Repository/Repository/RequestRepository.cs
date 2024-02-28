using HalloDoc.Entities.DataModels;
using HalloDoc.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.Repository
{
    public class RequestRepository: GenericRepository<Request>, IRequestRepository
    {
        private readonly ApplicationDbContext _context;
        public RequestRepository(ApplicationDbContext context):base(context)
        {
           _context = context;
        }
        public async Task<Request> CheckUserByEmail(string email)
        {
            Request user = _context.Requests.Where(x => x.Email == email).FirstOrDefault();

            return user;
        }
        public async Task<int> GetCountAsync(Expression<Func<Request, bool>> predicate)
        {
            using var context = new ApplicationDbContext();

            return await context.Requests.CountAsync(predicate);
        }
        public async Task<int> CheckUserByID(int id)
        {
            Request user = _context.Requests.Where(x => x.RequestId == id).FirstOrDefault();
            int userID = (int)user.UserId;
            return userID;
        }

    }
}
