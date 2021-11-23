using System.ComponentModel.DataAnnotations;

namespace CashTrack.Data.Entities
{
    public class IncomeSource
    {
        public int Id { get; set; }
        public IncomeCatagories Catagory { get; set; }
        [StringLength(100)]
        public string Source { get; set; }
    }
}
