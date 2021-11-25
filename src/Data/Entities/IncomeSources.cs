using System.ComponentModel.DataAnnotations;

namespace CashTrack.Data.Entities
{
    public class IncomeSources
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string Source { get; set; }
    }
}
