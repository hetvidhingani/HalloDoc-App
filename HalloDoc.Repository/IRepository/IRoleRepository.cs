using HalloDoc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.IRepository
{
    public interface IRoleRepository:IGenericRepository<Role>
    {
        //Task<List<Role>> GetRoles();
        IQueryable<Role> GetRolesProvider();
        Role findByName(string name);
    }
}
