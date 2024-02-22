using HalloDoc.Entities.DataModels;
using HalloDoc.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.Repository
{
    public class ConciergeRepository:GenericRepository<Concierge>,IConciergeRepository
    {
        public ConciergeRepository(ApplicationDbContext context):base(context) 
        { 

        }
       
    }
}
