using HalloDoc.Entities.DataModels;
using HalloDoc.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.Repository
{
    public class AspNetUserRepository : IAspNetUserRepository
    {
        private readonly ApplicationDbContext _context;
        public AspNetUserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAspnetUser(AspNetUser aspNetUser)
        {
            _context.Add(aspNetUser);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAspnetUser(AspNetUser aspNetUser)
        {
            _context.Update(aspNetUser);
            await _context.SaveChangesAsync();
        }
        public async Task<AspNetUser> CheckUserByEmail(string email)
        {
            AspNetUser aspNetUser = _context.AspNetUsers.Where(x => x.Email == email).FirstOrDefault();

            return  aspNetUser;
        }
        public async Task<AspNetUser> CheckUserForLogin(string email, string password)
        {
            AspNetUser aspNetUser = _context.AspNetUsers.Where(x => x.Email == email && x.PasswordHash == password).FirstOrDefault();

            return aspNetUser;
        }

    }
}
