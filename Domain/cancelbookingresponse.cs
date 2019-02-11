using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{

    public class CancelPNRResponse
    {
        public Error[] errors { get; set; }
        public bool success { get; set; }
        public int target { get; set; }
        public string uniqueID { get; set; }
        public long BookingRefID { get; set; }
        public long UserID { get; set; }
    }
}
