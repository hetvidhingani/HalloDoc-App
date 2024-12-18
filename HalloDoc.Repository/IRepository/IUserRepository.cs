﻿using HalloDoc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.IRepository
{
    public interface IUserRepository:IGenericRepository<User>
    {

      Task<User> CheckUserByEmail(string email);
    }
}
