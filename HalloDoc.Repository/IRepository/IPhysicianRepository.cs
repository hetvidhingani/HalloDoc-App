﻿using HalloDoc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.IRepository
{
    public interface IPhysicianRepository:IGenericRepository<Physician>
    {

        Task<List<Physician>> GetPhysician();
        Task<List<Physician>> GetPhysiciansByRegion(int regionId);
        Physician GetPhysician(string email);
    }
}
