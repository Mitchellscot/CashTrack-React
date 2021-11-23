using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace CashTrack.Data.Entities
{
    public class Expenses
    {
        public int Id { get; set; }
        [Required]
        public DateTime PurchaseDate { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [StringLength(50)]
        public Merchants Merchant { get; set; }
        [StringLength(255)]
        public string Notes { get; set; }
        public ExpenseSubCatagories Catagory { get; set; }
        public List<Tag> Tags { get; set; }
    }
}
