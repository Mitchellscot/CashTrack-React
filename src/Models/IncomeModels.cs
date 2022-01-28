using CashTrack.Models.Common;
using System;

namespace CashTrack.Models.IncomeModels;

public class IncomeRequest : TransactionRequest
{
}
public class IncomeResponse : PaginationResponse<IncomeListItem>
{
    public decimal TotalAmount { get; private set; }

    public IncomeResponse(int pageNumber, int pageSize, int totalCount, IncomeListItem[] listItems, decimal amount) : base(pageNumber, pageSize, totalCount, listItems)
    {
        TotalAmount = Math.Round(amount, 2);
    }
}

public record AddEditIncome
{
    public int? Id { get; set; }
    public DateTimeOffset IncomeDate { get; set; }
    public decimal Amount { get; set; }
    public string Notes { get; set; }
    public int CategoryId { get; set; }
    public int MerchantId { get; set; }
}

public class IncomeListItem
{
    public int Id { get; set; }
    public DateTimeOffset IncomeDate { get; set; }
    public decimal Amount { get; set; }
    public string Source { get; set; }
    public string Category { get; set; }
}

public class IncomeQuickView
{
    public int Id { get; set; }
    public string PurchaseDate { get; set; }
    public decimal Amount { get; set; }
    public string Category { get; set; }
}