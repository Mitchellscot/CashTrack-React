using System;
using System.Collections.Generic;

namespace CashTrack.Data.CsvFiles
{
    public static class CsvExtensions
    {
        public static IEnumerable<CsvModels.CsvExpense> ToExpenses(this IEnumerable<string> source)
        {
            int i = 0;
            foreach (var line in source)
            {
                var columns = line.Split(',');
                i++;
                yield return new CsvModels.CsvExpense()
                {
                    id = i,
                    purchase_date = DateTimeOffset.Parse(columns[0]),
                    amount = Decimal.Parse(columns[1]),
                    categoryid = columns[2] == "" ? null : Convert.ToInt32(columns[2]),
                    notes = columns[3] == "" ? null : columns[3],
                    merchantid = columns[4] == "" ? null : Convert.ToInt32(columns[4]),
                    exclude_from_statistics = ParseBoolean(columns[5])
                };
            }
        }
        public static IEnumerable<CsvModels.CsvExpenseMainCategory> ToExpenseMainCategory(this IEnumerable<string> source)
        {
            foreach (var line in source)
            {
                var columns = line.Split(',');
                yield return new CsvModels.CsvExpenseMainCategory()
                {
                    id = Convert.ToInt32(columns[0]),
                    main_category_name = columns[1],
                };
            }
        }
        public static IEnumerable<CsvModels.CsvExpenseSubCategory> ToExpenseSubCategory(this IEnumerable<string> source)
        {
            foreach (var line in source)
            {
                var columns = line.Split(',');
                yield return new CsvModels.CsvExpenseSubCategory()
                {
                    id = Convert.ToInt32(columns[0]),
                    sub_category_name = columns[1],
                    main_categoryid = Convert.ToInt32(columns[2]),
                    in_use = ParseBoolean(columns[3])
                };
            }
        }
        public static IEnumerable<CsvModels.CsvMerchant> ToMerchant(this IEnumerable<string> source)
        {
            foreach (var line in source)
            {
                var columns = line.Split(',');
                yield return new CsvModels.CsvMerchant()
                {
                    id = Convert.ToInt32(columns[0]),
                    name = columns[1],
                    suggest_on_lookup = ParseBoolean(columns[2]),
                    city = null,
                    state = null
                };
            }
        }
        public static IEnumerable<CsvModels.CsvIncomeCategory> ToIncomeCategory(this IEnumerable<string> source)
        {
            foreach (var line in source)
            {
                var columns = line.Split(',');
                yield return new CsvModels.CsvIncomeCategory()
                {
                    id = Convert.ToInt32(columns[0]),
                    category = columns[1]
                };
            }
        }
        public static IEnumerable<CsvModels.CsvIncomeSource> ToIncomeSource(this IEnumerable<string> source)
        {
            foreach (var line in source)
            {
                var columns = line.Split(',');
                yield return new CsvModels.CsvIncomeSource()
                {
                    id = Convert.ToInt32(columns[0]),
                    source = columns[1]
                };
            }
        }
        public static IEnumerable<CsvModels.CsvIncome> ToIncome(this IEnumerable<string> source)
        {
            int i = 0;
            foreach (var line in source)
            {
                var columns = line.Split(',');
                i++;
                yield return new CsvModels.CsvIncome()
                {
                    id = i,
                    income_date = DateTimeOffset.Parse(columns[0]),
                    amount = Convert.ToDecimal(columns[1]),
                    categoryid = Convert.ToInt32(columns[2]),
                    sourceid = Convert.ToInt32(columns[3]),
                    notes = columns[4] == "" ? null : columns[4]
                };
            }
        }
        private static bool ParseBoolean(string s) => s == "1";

    }
}
