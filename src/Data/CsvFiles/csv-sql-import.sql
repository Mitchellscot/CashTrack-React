--copy data from expense main category file
COPY expense_main_categories("Id", "Category")
FROM 'C:\Users\Public\db-files\expense-main-categories.csv'
DELIMITER ','
CSV HEADER;

--copy data from expense sub category file
COPY "expense_sub_categories"("Id", "Name", "CategoryId", "InUse")
FROM 'C:\Users\Public\db-files\expense-sub-categories.csv'
DELIMITER ','
CSV HEADER;

--copy data from merchants file
COPY "merchants"("Id", "Name", "SuggestOnLookup", "City", "State")
FROM 'C:\Users\Public\db-files\merchants.csv'
DELIMITER ','
CSV HEADER;

--copy data from expenses file
COPY expenses("PurchaseDate", "Amount", "CategoryId", "Notes", "MerchantId", "ExcludeFromStatistics")
FROM 'C:\Users\Public\db-files\expenses.csv'
DELIMITER ','
CSV HEADER;

--copy data from income categories file
COPY income_categories("Id", "Category")
FROM 'C:\Users\Public\db-files\income-categories.csv'
DELIMITER ','
CSV HEADER;

--copy data from income categories file
COPY "IncomeSource"("Id", "Category")
FROM 'C:\Users\Public\db-files\income-categories.csv'
DELIMITER ','
CSV HEADER;

--find all rows in 2016
select * from "Expenses" WHERE "PurchaseDate" between '2016-01-01'::date and '2016-12-31'::date ORDER BY "PurchaseDate";

--find all groceries
select * from "Expenses" where "categoryId" = 31;

--select statements
select * from "Expenses";
select * from "Users";
select * from "Income";
select * from "ExpenseCategories" ORDER BY "Id";

--danger zone
drop table "Expenses";
drop table "Users";
drop table "Income";
drop table "ExpenseCategories";