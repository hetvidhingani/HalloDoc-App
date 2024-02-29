using HalloDoc.Entities.DataModels;
using HalloDoc.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.Repository
{
    public class AdminRepository:GenericRepository<Admin>,IAdminRepository
    {
        private readonly ApplicationDbContext _context;
        public AdminRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Admin> CheckUserByEmail(string email)
        {
            Admin user = _context.Admins.Where(x => x.Email == email).FirstOrDefault();

            return user;
        }

    }
}
