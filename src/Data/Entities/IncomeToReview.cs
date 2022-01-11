using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashTrack.Data.Entities
{

    [Table("incomes_to_review")]
    public class IncomeToReview
    {
        public int id { get; set; }
        [Required]
        public DateTimeOffset income_date { get; set; }
        [Required]
        public decimal amount { get; set; }
        [StringLength(255)]
        public string notes { get; set; }
        public IncomeCategories suggested_category { get; set; }
        public IncomeSources suggested_source { get; set; }
        public bool is_reviewed { get; set; }
    }
}
