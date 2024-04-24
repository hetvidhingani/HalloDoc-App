using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entities.ViewModels
{
    public class BingMapsResponse
    {
        public ResourceSet[] ResourceSets { get; set; }

        public class ResourceSet
        {
            public string EstimatedTotal { get; set; }
            public Resource[] Resources { get; set; }

            public class Resource
            {
                public CoordinatePoint Point { get; set; }
            }
        }

        public class CoordinatePoint
        {
            public double[] Coordinates { get; set; }
        }

    }
}
