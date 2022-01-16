namespace CashTrack.Models.MerchantModels
{
    public record MerchantModels
    {
        public class Request
        {
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 25;
            public string SearchTerm { get; set; } = null;
        }
        public class Response
        {
            public int PageNumber { get; set; }
            public int TotalPages { get; set; }
            public Merchant[] Merchants { get; set; }
        }
        public record Merchant
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string City { get; set; }
            public bool IsOnline { get; set; }
            public int NumberOfExpenses { get; set; }
        }
        public record MerchantDetail
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public bool SuggestOnLookup { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Notes { get; set; }
            public bool IsOnline { get; set; }
            public int TotalSpent { get; set; }
            public int TotalSpentThisMonth { get; set; }
            public int TotalSpentThisYear { get; set; }
            public int MyProperty { get; set; }
            //maybe some more advanced stats... min, max, avg, total by year, etc. Seperate Tab. Make a seperate call as it would
            //take a long time to add all that up i'm sure.
        }
    }
}
