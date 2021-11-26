using System.ComponentModel.DataAnnotations.Schema;

namespace CashTrack.Data.Entities
{
    [Table("expense-tags")]
    public class ExpenseTags
    {
        public int ExpenseId { get; set; }
        public Expenses Expense { get; set; }
        public int TagId { get; set; }
        public Tags Tag { get; set; }
    }
}
