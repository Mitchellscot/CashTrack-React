using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashTrack.Data.Entities
{
    [Table("expense-sub-categories")]
    public class ExpenseSubCategories
    {
        public int Id { get; set; }
        [StringLength(50)]
        [Required]
        public string Name { get; set; }
        public ExpenseMainCategories Category { get; set; }
        public bool InUse { get; set; } = true;
        public ICollection<Expenses> Expenses { get; set; }
    }
}
