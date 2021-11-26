using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashTrack.Data.Entities
{
    [Table("expenses")]
    public class Expenses
    {
        public int Id { get; set; }
        [Required]
        public DateTime PurchaseDate { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [StringLength(50)]
        public Merchants Merchant { get; set; }
        [StringLength(255)]
        public string Notes { get; set; }
        public ExpenseSubCategories Category { get; set; }
        public bool ExcludeFromStatistics { get; set; }
        public ICollection<ExpenseTags> ExpenseTags { get; set; }

    }
}
