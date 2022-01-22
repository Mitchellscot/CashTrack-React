#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashTrack.Data.Entities
{
    [Table("expenses")]
    public class Expenses : IEntity
    {
        private DateTimeOffset _purchase_date;
        public int id { get; set; }
        [Required]
        public DateTimeOffset purchase_date
        {
            get { return _purchase_date; }
            set { _purchase_date = value.ToUniversalTime(); }
        }
        [Required]
        public decimal amount { get; set; }
        [StringLength(50)]
        public int? merchantid { get; set; }
        public Merchants? merchant { get; set; }
        [StringLength(255)]
        public string? notes { get; set; }
        public int? categoryid { get; set; }
        public ExpenseSubCategories? category { get; set; }
        public bool exclude_from_statistics { get; set; }
        public ICollection<ExpenseTags>? expense_tags { get; set; }

    }
}
