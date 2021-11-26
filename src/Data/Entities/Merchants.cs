using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashTrack.Data.Entities
{
    [Table("merchants")]
    public class Merchants
    {
        public int Id { get; set; }
        [StringLength(250)]
        [Required]
        public string Name { get; set; }
        public bool SuggestOnLookup { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public ICollection<Expenses> Expenses { get; set; }
    }
}