using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashTrack.Data.Entities
{
    [Table("tags")]
    public class Tags
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string TagName { get; set; }
        public ICollection<ExpenseTags> ExpenseTags { get; set; }
    }
}