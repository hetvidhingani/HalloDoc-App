﻿using HalloDoc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.IRepository
{
    public interface IRequestClientRepository:IGenericRepository<RequestClient>
    {
        Task<RequestClient> CheckUserByEmail(string email);
        Task<RequestClient> CheckUserByID(int id);
        Task<RequestClient> GetRequestIDByClientID(int id);
        string GetRequestClientIDByRequestID(int id);
        Task<RequestClient> CheckUserByClientID(int id);
    }
}
