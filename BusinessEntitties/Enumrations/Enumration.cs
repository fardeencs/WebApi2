namespace BusinessEntitties.Enumrations
{
    public enum BookingStatusCode
    {
        RQ = 1, HQ, OK
    }

    public enum PaymentStatus
    {
        PENDING = 1, DONE = 2
    }

    public enum ServiceType
    {
        FLIGHT = 1
    }

    public enum CommissionTypes
    {
        Value = 2, Percentage = 1
    }
    public enum MarkUpType
    {
        Percentage = 1, Value = 2
    }
    public enum Titles
    {
        Mr = 1, Mrs = 2, Ms = 3, Miss = 4, Mstr = 5, Dr = 6, HE = 7, HH = 8
    }
    public enum PaxTypes
    {
        ADT = 1, CHD = 2, INF = 3
    }
    public enum SupplierCode
    {
        MIS001, AMA001, PYT001,GTA001
    }
}
