using Assignment.Entities.DataModels;
using Assignment.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Repository.Repository
{
    public class DoctorRepository:GenericRepository<Doctor>,IDoctorRepository
    {
        private readonly ApplicationDbContext _context;
        public DoctorRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
