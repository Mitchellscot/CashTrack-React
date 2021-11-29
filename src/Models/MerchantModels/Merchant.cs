namespace CashTrack.Models.MerchantModels
{
    public record Merchant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool SuggestOnLookup { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
