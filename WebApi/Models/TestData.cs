using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class TestData
    {
        public Domain.Rootobject FilteredData { get; set; }
        public List<Domain.Rootobject> NonFilteredData { get; set; }
    }
}