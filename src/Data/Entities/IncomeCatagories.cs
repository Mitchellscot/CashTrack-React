using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashTrack.Data.Entities
{
    [Table("income-categories")]
    public class IncomeCategories
    {
        public int Id { get; set; }
        [StringLength(50)]
        [Required]
        public string Category { get; set; }
    }
}
