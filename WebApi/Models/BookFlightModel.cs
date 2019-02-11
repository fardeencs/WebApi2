
namespace WebApi.Models
{

    using Common;
    using MediatR;
    using System.Collections.Generic;
    using BusinessEntitties;

    public class BookFlightModel : IAsyncRequest<ResponseObject>
    {
        public BookFlightEntity BookFlightEntity { get; set; }
        public TotalFareGroup Totalfaregroup { get; set; }
        public List<AirBagDetails> AirBagDetails { get; set; }
        public List<Fareruleseg> Fareruleseg { get; set; }
        public List<CostBreakuppax> CostBreakuppax { get; set; }
        public List<costAirMarkUp> costAirMarkUp { get; set; }
    }
}


