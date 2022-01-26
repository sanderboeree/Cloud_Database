namespace OnlineStore.Api.Domain.Orders
{
    public enum PaymentType
    {
        NotSpecified = 0,
        IDEAL = 1,
        Paypal = 2,
        CreditCard = 3,
        DebitCard = 4
    }
}