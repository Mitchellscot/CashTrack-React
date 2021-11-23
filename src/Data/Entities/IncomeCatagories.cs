using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CashTrack.Data.Entities
{
    public class IncomeCatagories
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string Catagory { get; set; }
        public ICollection<Incomes> Income {get; set;}
        public ICollection<IncomeSource> IncomeSources { get; set;}          
    }
}
