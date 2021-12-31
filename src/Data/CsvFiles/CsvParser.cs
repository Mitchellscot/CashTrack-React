using CashTrack.Data.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CashTrack.Data.CsvFiles
{
    public class CsvParser
    {
        public static List<CsvModels.CsvExpense> ProcessExpenseFile(string path)
        {
            return File.ReadAllLines(path).Skip(1).Where(line => line.Length > 1).ToExpenses().ToList();
        }
        public static List<CsvModels.CsvExpenseMainCategory> ProcessMainCategoryFile(string path)
        { 
            return File.ReadAllLines(path).Skip(1).Where(l => l.Length > 1).ToExpenseMainCategory().ToList();
        }
    }
}
