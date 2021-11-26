using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashTrack.Data.Entities
{
    [Table("expense_sub_categories")]
    public class ExpenseSubCategories
    {
        public int id { get; set; }
        [StringLength(50)]
        [Required]
        public string name { get; set; }
        public ExpenseMainCategories category { get; set; }
        public bool in_use { get; set; } = true;
        public ICollection<Expenses> expenses { get; set; }
    }
}
