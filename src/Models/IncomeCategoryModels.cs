﻿using CashTrack.Models.Common;

namespace CashTrack.Models.IncomeCategoryModels;

public class IncomeCategoryModels
{
    public class Request : PaginationRequest
    {
    }
    public class Response : PaginationResponse<IncomeCategoryListItem>
    {
    }
}
public record AddEditIncomeCategory
{
    public int? Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool InUse { get; set; } = true;
}
public record IncomeCategoryListItem
{
    public int Id { get; set; }
    public string Name { get; set; }
}
public record IncomeCategoryDetail
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool InUse { get; set; } = true;
    //maybe some other cool properties... how many incomes related to this, total amount... maybe a yearly graph?
    //Get all expenses by income category and compare with income sources. How many gifts came from my parents, etc.
}
