using HalloDoc.Entities.DataModels;
using HalloDoc.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.Repository
{
    public class AspNetUserRepository : GenericRepository<AspNetUser>,IAspNetUserRepository
    {
        private readonly ApplicationDbContext _context;
        public AspNetUserRepository(ApplicationDbContext context) : base(context)
        {
           _context = context;
        }
        public async Task<AspNetUser> CheckUserByEmail(string email)
        {
            AspNetUser user = _context.AspNetUsers.Where(x => x.Email == email).FirstOrDefault();

            return user;
        }
        public async Task<AspNetUser> Login(string email,string password)
        {
            AspNetUser user1 = await CheckUserByEmail(email);

            var pwd =DecodeFrom64(user1.PasswordHash);
            AspNetUser user = _context.AspNetUsers.Where(x => x.Email == email && pwd==password).FirstOrDefault();
            return user;
        }
       
    }
}
