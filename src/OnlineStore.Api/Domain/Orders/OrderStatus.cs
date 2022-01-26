namespace OnlineStore.Api.Domain.Orders
{
    public enum OrderStatus
    {
        NotSpecified = 0,
        AwaitingPayment = 1,
        PreparingSending = 2,
        ReadyForSending = 3,
        Delivering = 4,
        Completed = 5,
        RefundedPartially = 6,
        Refunded = 7,
        Cancelled = 8,
        Failed = 9,
        Expired = 10
    }
}