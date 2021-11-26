using System;

namespace CashTrack.Models.expenses
{
    public abstract class Transaction
    {
        abstract public int Id { get; set; }
        abstract public DateTime PurchaseDate { get; set; }
        abstract public Decimal Amount { get; set; }
        abstract public string Notes { get; set; }


    }
}
