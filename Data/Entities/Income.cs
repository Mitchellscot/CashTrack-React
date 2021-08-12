using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CashTrack.Data.Entities
{
    public class Income
    {
        public int Id { get; set; }
        [Required]
        public DateTime IncomeDate { get; set; }
        [Required]
        public decimal Amount { get; set; }

        [StringLength(255)]
        public string Notes { get; set; }
        public IncomeCatagory Catagory { get; set; }
    }
}
