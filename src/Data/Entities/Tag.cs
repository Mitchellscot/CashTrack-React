using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace CashTrack.Data.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string TagName { get; set; }
        public ICollection<Expenses> Expenses { get; set; }
    }
}