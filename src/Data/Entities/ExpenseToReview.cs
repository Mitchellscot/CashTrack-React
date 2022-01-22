using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashTrack.Data.Entities
{
    [Table("expenses_to_review")]
    public class ExpenseToReview : IEntity
    {
        public int id { get; set; }
        [Required]
        public DateTimeOffset purchase_date { get; set; }
        [Required]
        public decimal amount { get; set; }
        [StringLength(255)]
        public string notes { get; set; }
        public ExpenseSubCategories suggested_category { get; set; }
        public Merchants suggested_merchant { get; set; }
        public bool is_reviewed { get; set; }
    }
}
