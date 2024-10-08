﻿namespace Store.Domain.Entities.Model
{
    /// <summary>
    /// Used for describing payment of order.
    /// </summary>
    public class PaymentDetails : Entity<Guid>
    {
        public decimal AmountPayed { get; set; }
        public Guid OrderId { get; set; }
        public virtual Order Order { get; set; }
        public Guid CashDepositId { get; set; }
        public virtual CashDeposit CashDeposit { get; set; }
    }
}
