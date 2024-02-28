using HalloDoc.Entities.DataModels;
using HalloDoc.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.Repository
{
    public class UserRepository :GenericRepository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context):base(context) 
        {
            _context = context;
        }
        public async Task<User> CheckUserByEmail(string email)
        {
            User user = _context.Users.Where(x => x.Email == email).FirstOrDefault();

            return user;
        }
        

    }
}
