using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntitties.Insurance
{
    public class InsuranceSupplierCredentials
    {

        public string BaseUrl { get; set; }
        public string AgencyCode { get; set; }
        public string ClientID { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string Language { get; set; }
        public string Currency { get; set; }
        public string Country { get; set; }
        public string RequestMode { get; set; }
    }
}
