using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using HalloDoc.Entities.DataModels;

using System.Threading.Tasks;

namespace HalloDoc.Repository.IRepository
{
    public interface IRegionRepository:IGenericRepository<Region>
    {
        Task<List<Region>> GetRegions();
        List<Region> GetRegion();
        Task<string> FindState(int? regionID);
    }
}
