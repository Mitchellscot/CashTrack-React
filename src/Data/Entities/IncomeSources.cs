using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashTrack.Data.Entities
{
    [Table("income-sources")]
    public class IncomeSources
    {
        public int Id { get; set; }
        [StringLength(100)]
        [Required]
        public string Source { get; set; }
    }
}
