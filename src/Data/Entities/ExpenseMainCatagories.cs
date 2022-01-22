using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashTrack.Data.Entities
{
    [Table("expense_main_categories")]
    public class ExpenseMainCategories : IEntity
    {
        public int id { get; set; }
        [StringLength(50)]
        [Required]
        public string main_category_name { get; set; }
        public ICollection<ExpenseSubCategories> sub_categories { get; set; }
    }
}
