using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    using BusinessEntitties;
    public class SupplierTripDetailsResponse
    {
        public List<string> errors { get; set; }
        public Data data { get; set; }
    }
    public class Data
    {
        public List<string> errors { get; set; }
        public bool success { get; set; }
        public string target { get; set; }
        public Travelitinerary travelItinerary { get; set; }
    }
}
