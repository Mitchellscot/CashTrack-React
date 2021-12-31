#nullable enable
using System;

namespace CashTrack.Data.CsvFiles
{
    public class CsvModels
    {
        //these models are used when pulling data from the CSV files and inserting it into the database
        public class CsvExpense
        {
            public int id { get; set; }
            public DateTimeOffset? purchase_date { get; set; }
            public decimal? amount { get; set; }
            public int? categoryid { get; set; }
            public string? notes { get; set; }
            public int? merchantid { get; set; }
            public bool? exclude_from_statistics { get; set; }
        }
        public class CsvExpenseMainCategory
        {
            public int id { get; set; }
            public string? main_category_name { get; set; }
        }
    }
}
