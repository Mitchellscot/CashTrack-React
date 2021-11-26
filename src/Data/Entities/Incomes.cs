using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashTrack.Data.Entities
{
    [Table("incomes")]
    public class Incomes
    {
        public int Id { get; set; }
        [Required]
        public DateTime IncomeDate { get; set; }
        [Required]
        public decimal Amount { get; set; }

        [StringLength(255)]
        public string Notes { get; set; }
        public IncomeCategories Category { get; set; }
        public IncomeSources Source { get; set; }
    }
}
