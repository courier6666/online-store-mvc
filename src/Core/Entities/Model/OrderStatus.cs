namespace Store.Domain.Entities
{
    /// <summary>
    /// Enum that represents current status of order.
    /// </summary>
    public enum OrderStatus
    {

        New = 0,
        CancelledByAdmin = 1,
        PaymentTransferred = 2,
        PaymentReceived = 3,
        Sent = 4,
        CancelledByUser = 5,
        Received = 6,
        Completed = 7
    }

}
