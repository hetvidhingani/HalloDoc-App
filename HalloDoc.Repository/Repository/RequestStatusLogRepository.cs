using HalloDoc.Entities.DataModels;
using HalloDoc.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.Repository
{
    public class RequestStatusLogRepository : GenericRepository<RequestStatusLog>, IRequestStatusLogRepository
    {
        private readonly ApplicationDbContext _context;
        public RequestStatusLogRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<RequestStatusLog> CheckByRequestID(int id)
        {
            RequestStatusLog user = _context.RequestStatusLogs.Where(x => x.RequestId == id).FirstOrDefault();
            return user;
        }
        public async Task<List<RequestStatusLog>> GetNotes(int id)
        {
            return _context.RequestStatusLogs.Where(x => x.RequestId == id).ToList();
        }
        public async Task<List<Physician>> GetPhysicianNames(int requestId)
        {
            var physicianIds = await _context.RequestStatusLogs
                .Where(reqLog => reqLog.RequestId == requestId)
                .Select(reqLog => reqLog.TransToPhysicianId)
                .ToListAsync();

            var physicianNames = await _context.Physicians
                .Where(p => physicianIds.Contains(p.PhysicianId))
                .Select(p => new Physician { PhysicianId = p.PhysicianId, FirstName = p.FirstName })
                .ToListAsync();

            return physicianNames;
        }
    }
    }
