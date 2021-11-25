using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CashTrack.Data.Entities
{
    public class IncomeCategories
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string Category { get; set; }
        public ICollection<Incomes> Income {get; set;}
        public ICollection<IncomeSources> IncomeSource { get; set;}          
    }
}
