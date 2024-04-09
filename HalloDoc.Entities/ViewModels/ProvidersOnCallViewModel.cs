using HalloDoc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class ProvidersOnCallViewModel
    {
        public List<PhysicianOnCallModal> PaggingData { get; set;}
        public List<Region> regions { get; set;}
        public int regionId { get; set;}
       public List<int> physicianOnCall {  get; set;}
    }
    public class PhysicianOnCallModal
    {
        public bool IsOnCall { get; set; }
        public int physicianId { get; set; }
        public string physicianName { get; set; }
        public string Photo {  get; set; }
    }
}
