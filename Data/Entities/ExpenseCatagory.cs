using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CashTrack.Data.Entities
{
    //sub catagory
    public class ExpenseCatagory
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public ExpenseMainCatagory Catagory { get; set; }
        public bool InUse { get; set; } = true;
    }
}
