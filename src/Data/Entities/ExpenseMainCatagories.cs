using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashTrack.Data.Entities
{
    [Table("expense-main-categories")]
    public class ExpenseMainCategories
    {
        public int Id { get; set; }
        [StringLength(50)]
        [Required]
        public string Category { get; set; }
        public ICollection<ExpenseSubCategories> SubCategories { get; set;}
    }
}
