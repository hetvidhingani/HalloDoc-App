using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class ConcludeCareViewModel
    {
        public string filePath { get; set; }
        public int requestId { get; set; }
        public string? note { get; set; }
        public string downloadPath { get; set; }
        public string username { get; set; }
    }
}
