using CashTrack.Data.Entities.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashTrack.Data.Entities
{

    [Table("incomes_to_review")]
    public class IncomeToReview : Transactions
    {
        public IncomeCategories suggested_category { get; set; }
        public IncomeSources suggested_source { get; set; }
        public bool is_reviewed { get; set; }
    }
}
