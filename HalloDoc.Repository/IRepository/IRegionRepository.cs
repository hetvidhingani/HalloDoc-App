using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.IRepository
{
    public interface IRegionRepository:IGenericRepository<Region>
    {
        Task<List<Entities.DataModels.Region>> GetRegions();
    }
}
