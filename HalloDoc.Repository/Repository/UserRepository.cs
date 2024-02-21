using HalloDoc.Entities.DataModels;
using HalloDoc.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddUser(User users)
        {
            _context.Add(users);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateUser(User user)
        {
            _context.Update(user);
            await _context.SaveChangesAsync();
        }
        public async Task<User> CheckUserByEmail(string email)
        {
            User user = _context.Users.Where(x => x.Email == email).FirstOrDefault();

            return user;
        }
        public async Task<User> GetUser(int? userID)
        {
            User user = _context.Users.Where(x => x.UserId == userID).FirstOrDefault();

            return user;
        }

    }
}
