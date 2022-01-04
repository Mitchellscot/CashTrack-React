using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashTrack.Data.Entities
{
    [Table("expenses_to_review")]
    public class ExpenseToReview
    {
        public int id { get; set; }
        [Required]
        public DateTimeOffset purchase_date { get; set; }
        [Required]
        public decimal amount { get; set; }
        [StringLength(255)]
        public string notes { get; set; }
        public ExpenseSubCategories SuggestedSubCategory { get; set; }
        public Merchants SuggestedMerchant { get; set; }
        public bool IsReviewed {get; set;}
    }
}
