using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CashTrack.Data.Entities
{
    public class ExpenseMainCategories
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string Category { get; set; }
        public ICollection<ExpenseSubCategories> SubCategories { get; set;}
    }
}
