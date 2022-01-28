using CashTrack.Models.Common;
using CashTrack.Models.TagModels;
using System;
using System.Collections.Generic;


namespace CashTrack.Models.ExpenseModels;


public class ExpenseRequest : PaginationRequest
{
    public DateOptions DateOptions { get; set; }
    public DateTimeOffset BeginDate { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset EndDate { get; set; } = DateTimeOffset.UtcNow;
}
public class ExpenseResponse : PaginationResponse<ExpenseListItem>
{
    public decimal TotalAmount { get; private set; }
    public ExpenseResponse(int pageNumber, int pageSize, int totalCount, ExpenseListItem[] listItems, decimal amount) : base(pageNumber, pageSize, totalCount, listItems)
    {
        TotalAmount = Math.Round(amount, 2);
    }
}
public class AmountSearchRequest : PaginationRequest
{
    private decimal _query;
    new public decimal Query
    {
        get { return _query; }
        set
        {
            _query = Decimal.Round(value, 2);
        }
    }
}

public record AddEditExpense
{
    public int? Id { get; set; }
    public DateTimeOffset PurchaseDate { get; set; }
    public decimal Amount { get; set; }
    public string Notes { get; set; }
    public int? MerchantId { get; set; }
    //figure this out after you get Tags CRUD set up
    //public ICollection<Tag> Tags { get; set; }
    public int SubCategoryId { get; set; }
}
public record ExpenseListItem
{
    public int Id { get; set; }
    public DateTimeOffset PurchaseDate { get; set; }
    public decimal Amount { get; set; }
    //I will probably remove Notes and add that to a detail view to be viewed in a modal
    public string Notes { get; set; }
    public string Merchant { get; set; }
    public ICollection<TagModel> Tags { get; set; }
    public string SubCategory { get; set; }
    public string MainCategory { get; set; }
}
public record ExpenseQuickView
{
    public int Id { get; set; }
    public string PurchaseDate { get; set; }
    public decimal Amount { get; set; }
    public string SubCategory { get; set; }
}

