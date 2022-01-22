namespace CashTrack.Models.SubCategoryModels;

public record SubCategoryModels
{
    public record Request
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 25;
        public string SearchTerm { get; set; } = null;
    }
    public record Response
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 25;
        public int TotalPages { get; set; }
        public decimal TotalSubCategories { get; set; }
        public SubCategoryListItem[] SubCategories { get; set; }
    }
}
public record AddEditSubCategory
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int MainCategoryId { get; set; }
    public string Notes { get; set; }
    public bool InUse { get; set; }
}

public record SubCategoryListItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string MainCategoryName { get; set; }
    public int NumberOfExpenses { get; set; }
}
public record SubCategoryDetail
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string MainCategoryName { get; set; }
    public string Notes { get; set; }
    public bool InUse { get; set; }
    //Some ideas....
    //expenses and stats by year (like merchants)
    //pie chart showing merchant breakdown showing amount spent at every merchant
    //another one showing # of purchases in given category by merchant
    //Chart showing average monthly cost ? (for budgeting)
    //might have more ideas when I get the app up and running
    //recent expenses in a give category with link to view all expenses by given category

}