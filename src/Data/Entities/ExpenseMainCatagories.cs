using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CashTrack.Data.Entities
{
    public class ExpenseMainCatagories
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string Catagory { get; set; }
        public ICollection<ExpenseSubCatagories> SubCatagories { get; set;}
    }
}
