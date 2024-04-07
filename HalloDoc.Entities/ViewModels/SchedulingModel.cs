using HalloDoc.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class SchedulingModel
    {
        public List<Resources> resources { get; set; }
        public List<Events> events { get; set; }
        public int RegionId {  get; set; }
        public List<Region> regions { get; set; }
    }
    public class Resources
    {
        public string id { get; set; }
        public string title { get; set; }
        public string avtar { get; set; }
    }
    public class Events
    {
        public string id { get; set; }
        public string resourceId { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string title { get; set; }
        public string color { get; set; }

    }
}
