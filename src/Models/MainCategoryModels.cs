using CashTrack.Models.SubCategoryModels;

namespace CashTrack.Models.MainCategoryModels;

public class MainCategoryModels
{
    public record Request()
    {
        public string SearchTerm { get; set; }
    }
    public record Response()
    {
        public int TotalMainCategories { get; set; }
        public MainCategoryListItem[] MainCategories { get; set; }
    }
}
public record MainCategoryListItem
{
    public int MyProperty { get; set; }
    public string Name { get; set; }
    public int NumberOfSubCategories { get; set; }
}
public record AddEditMainCategory
{
    public int Id { get; set; }
    public string Name { get; set; }
}
public record MainCategoryDetail
{
    public int Id { get; set; }
    public string Name { get; set; }
    public SubCategoryListItem[] SubCategories { get; set; }
    //Think of a stats object like merchant detail with every year and a bar graph of expenses by sub category for each year,
}

