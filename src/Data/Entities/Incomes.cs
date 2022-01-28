using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashTrack.Data.Entities
{
    [Table("incomes")]
    public class Incomes : IEntity
    {
        public int id { get; set; }
        [Required]
        public DateTimeOffset income_date { get; set; }
        [Required]
        public decimal amount { get; set; }

        [StringLength(255)]
        public string notes { get; set; }
        public int? categoryid { get; set; }
        public IncomeCategories category { get; set; }
        public int? sourceid { get; set; }
        public IncomeSources source { get; set; }
    }
}
