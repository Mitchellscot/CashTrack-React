using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashTrack.Data.Entities
{
    [Table("income_sources")]
    public class IncomeSources
    {
        public int id { get; set; }
        [StringLength(100)]
        [Required]
        public string source { get; set; }
    }
}
