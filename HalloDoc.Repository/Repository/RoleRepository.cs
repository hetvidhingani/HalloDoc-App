using HalloDoc.Entities.DataModels;
using HalloDoc.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.Repository
{
    public class RoleRepository:GenericRepository<Role> ,IRoleRepository
    {
        private readonly ApplicationDbContext _context;
        public RoleRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<Role>> GetRoles()
        {
            return _context.Roles.ToList();
        }
        public  IQueryable<Role> GetRolesProvider()
        {
            return _context.Roles.Where(u=>u.AccountType == 2);
        }
        public Role findByName(string name)
        {
            Role data = _context.Roles.Where(u=> u.Name == name).FirstOrDefault();
            return data;
        }
    }
}
