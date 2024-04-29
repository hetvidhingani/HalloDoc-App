using HalloDoc.Entities.DataModels;
using HalloDoc.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.Repository
{
    public class QuaterSheetRepository:GenericRepository<QuarterSheet>,IQuaterSheetRepository
    {
        private readonly ApplicationDbContext _context;
        public QuaterSheetRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public QuarterSheet check(string startdate)
        {
            DateOnly date = DateOnly.Parse(startdate);
            QuarterSheet result = _context.QuarterSheets.Where(x => x.StartDate == date).FirstOrDefault();
            if (result != null)
            {
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
