using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BusinessEntitties;
namespace WebApi.Models
{
    using Common;
    using MediatR;
    public class GetTripDetailsModel : IAsyncRequest<ResponseObject>
    {
        public Connectiontodbreq ConnectiontoDBreq { get; set; }
    } 
}