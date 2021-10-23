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
        public int MyProperty { get; set; }
        public string TagName { get; set; }
    }
}