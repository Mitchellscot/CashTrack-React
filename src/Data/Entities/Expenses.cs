using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashTrack.Data.Entities
{
    [Table("expenses")]
    public class Expenses
    {
        public int id { get; set; }
        [Required]
        public DateTimeOffset purchase_date { get; set; }
        [Required]
        public decimal amount { get; set; }
        [StringLength(50)]
        public Merchants merchant { get; set; }
        [StringLength(255)]
        public string notes { get; set; }
        public ExpenseSubCategories category { get; set; }
        public bool exclude_from_statistics { get; set; }
        public ICollection<ExpenseTags> expense_tags { get; set; }

    }
}
