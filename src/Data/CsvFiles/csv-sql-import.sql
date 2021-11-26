--copy data from expense main category file
COPY "expense-main-categories"("Id", "Category")
FROM 'C:\Users\Public\db-files\expense-main-categories.csv'
DELIMITER ','
CSV HEADER;

--copy data from expense sub category file
COPY "expense-sub-categories"("Id", "Name", "CategoryId", "InUse")
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
COPY "income-categories"("Id", "Category")
FROM 'C:\Users\Public\db-files\income-categories.csv'
DELIMITER ','
CSV HEADER;

--copy data from income sources file
COPY "income-sources"("Id", "Source")
FROM 'C:\Users\Public\db-files\income-sources.csv'
DELIMITER ','
CSV HEADER;

--copy data from incomes file
COPY "incomes"("IncomeDate", "Amount", "CategoryId", "SourceId", "Notes")
FROM 'C:\Users\Public\db-files\incomes.csv'
DELIMITER ','
CSV HEADER;

--find all rows in 2021
select * from "expenses" WHERE "PurchaseDate" between '2021-01-01'::date and '2021-12-31'::date ORDER BY "PurchaseDate";

--find all groceries
select * from "Expenses" where "categoryId" = 31;

--select statements
select * from expenses;
select * from "expense-main-categories";
select * from "expense-sub-categories";
select * from "expense-tags";
select * from "income-categories";
select * from "income-sources";
select * from incomes;
select * from merchants;
select * from tags;
select * from users;

--danger zone

--delete all data in a table
delete from expenses;
delete from "expense-main-categories";
delete from "expense-sub-categories";
delete from "expense-tags";
delete from "income-categories";
delete from "income-sources";
delete from incomes;
delete from merchants;
delete from tags;
delete from users;

--drop a table
drop table expenses;
drop table "expense-main-categories";
drop table "expense-sub-categories";
drop table "expense-tags";
drop table "income-categories";
drop table "income-sources";
drop table incomes;
drop table merchants;
drop table tags;
drop table users;

--tag example. Set a tag named "Mitch" to every expense in the dentist category with the name "Mitch" in the notes
select * from "expenses" WHERE "Notes" ILIKE '%mitch%' AND "CategoryId"=14;

insert into "tags"("TagName") VALUES ('Mitch') RETURNING "Id";

--these ExpenseIds might be different. Enter in the ids that you get from the above returning statement
insert into "expense-tags"("ExpenseId", "TagId") VALUES 
	(659, 1),
	(1532, 1),
	(3028, 1),
	(3048, 1),
	(3094, 1),
	(3098, 1),
	(3108, 1);

--confirm tag works be returning all tags with the tag name Mitch
select expenses."Id", expenses."PurchaseDate", expenses."Amount", expenses."MerchantId", expenses."Notes", expenses."CategoryId"
from expenses
join "expense-tags" ON "expense-tags"."ExpenseId"=expenses."Id"
join tags ON "expense-tags"."TagId"=tags."Id"
where tags."TagName"= 'Mitch'
group by expenses."Id", expenses."PurchaseDate", expenses."Amount", expenses."MerchantId", expenses."Notes", expenses."CategoryId"
order by "PurchaseDate" ASC;