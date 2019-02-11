using Autofac;
using Logic;
using Logic.Insurance;
using Logic.Interface;
using Logic.Interface.Insurance;
using Logic.Interface.sightSeeing;
using Logic.Interface.Transfer;
using Logic.SightSeeing;
using Logic.Transfer;

namespace WebApi.App_Start
{
    public class LogicConfig
    {
        public static void RegisterContext(ContainerBuilder builder)
        {
            builder.RegisterType<UserServices>().As<IUserServices>();
            builder.RegisterType<SupplierAgencyServices>().As<ISupplierAgencyServices>();
            builder.RegisterType<BookingServices>().As<IBookingServices>();
            builder.RegisterType<SightseeingSupplierDetails>().As<ISightseeingSupplierDetails>();
            builder.RegisterType<TransferSupplierDetails>().As<ITranserSupplierDetails>();
            builder.RegisterType<InsuranceSupplierDetails>().As<IInsuranceSupplierDetails>();
            builder.RegisterType<AgenciesBasicDetails>().As<IAgenciesBasicDetails>();
        }
    }
}