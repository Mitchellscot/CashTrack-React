using AutoMapper;
using CashTrack.Data.Entities;
using CashTrack.Models.Common;
using CashTrack.Models.ExpenseModels;
using CashTrack.Repositories.ExpenseRepository;
using System;
using System.Threading.Tasks;

namespace CashTrack.Services.ExpenseService
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepo;
        private readonly IMapper _mapper;

        public ExpenseService(IExpenseRepository expenseRepository, IMapper mapper)
        {
            _expenseRepo = expenseRepository;
            _mapper = mapper;
        }
        public async Task<ExpenseTransaction> GetExpenseByIdAsync(int id)
        {
            var singleExpense = await _expenseRepo.GetExpenseById(id);

            return _mapper.Map<ExpenseTransaction>(singleExpense);
        }
        public async Task<ExpenseModels.Response> GetExpensesAsync(ExpenseModels.Request request) => request.DateOptions switch
        {
            //1
            DateOptions.All => await GetAllExpensesAsync(request),
            //2
            //Required Begin Date
            DateOptions.SpecificDate => await GetExpensesFromSpecificDateAsync(request),
            //3
            // required begin date
            DateOptions.SpecificMonthAndYear => await GetExpensesForSpecificMonthAsync(request),
            //4
            //requires a begin date
            DateOptions.SpecificQuarter => await GetExpensesForQuarterAsync(request),
            //5
            // Requires begin date
            DateOptions.SpecificYear => await GetExpensesForAYearAsync(request),
            //6
            //begindate and enddate required.
            DateOptions.DateRange => await GetExpensesByDateRange(request),
            //7
            DateOptions.Last30Days => await GetExpensesFromLast30Days(request),
            //8
            DateOptions.CurrentMonth => await GetExpensesFromCurrentMonth(request),
            //9
            DateOptions.CurrentQuarter => await GetExpensesFromCurrentQuarter(request),
            //10
            DateOptions.CurrentYear => await GetExpensesFromCurrentYear(request),

            _ => throw new ArgumentException($"DateOption type not supported {request.DateOptions}", nameof(request.DateOptions))

        };
        internal async Task<ExpenseModels.Response> GetAllExpensesAsync(ExpenseModels.Request request)
        {
            var expenseTransactions = await _expenseRepo.GetAllExpensesPagination(request.PageNumber, request.PageSize);
            var pagesAndExpenses = await GetTotalPages(request.PageSize);
            var response = new ExpenseModels.Response
            {
                TotalPages = pagesAndExpenses.totalPages,
                PageSize = request.PageSize,
                TotalExpenses = pagesAndExpenses.totalExpenses,
                PageNumber = request.PageNumber,
                Expenses = _mapper.Map<ExpenseTransaction[]>(expenseTransactions)
            };
            return response;
        }
        internal async Task<(int totalPages, int totalExpenses)> GetTotalPages(int pageSize, DateTimeOffset? beginDate = null, DateTimeOffset? endDate = null)
        {
            decimal numberOfExpenses = 0;
            if (beginDate == null && endDate == null)
            {
                numberOfExpenses = await _expenseRepo.GetCountOfAllExpenses();
            }
            else if (beginDate != null && endDate == null)
            {
                numberOfExpenses = await _expenseRepo.GetCountOfExpensesForSpecificDate(beginDate.Value);
            }
            else if (beginDate != null && endDate != null)
            {
                numberOfExpenses = await _expenseRepo.GetCountOfExpensesBetweenTwoDates(beginDate.Value, endDate.Value);
            }
            var totalPages = (int)Math.Ceiling(numberOfExpenses / (decimal)pageSize);
            return (totalPages == 0 ? 1 : totalPages, (int)numberOfExpenses);
        }
        internal async Task<ExpenseModels.Response> GetExpensesFromSpecificDateAsync(ExpenseModels.Request request)
        {
            var expenseTransactions = await _expenseRepo.GetExpensesFromSpecificDatePagination(request.BeginDate, request.PageNumber, request.PageSize);
            var pagesAndExpenses = await GetTotalPages(request.PageSize, request.BeginDate);
            var response = new ExpenseModels.Response
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalPages = pagesAndExpenses.totalPages,
                TotalExpenses = pagesAndExpenses.totalExpenses,
                Expenses = _mapper.Map<ExpenseTransaction[]>(expenseTransactions)
            };
            return response;
        }
        internal async Task<ExpenseModels.Response> GetExpensesForSpecificMonthAsync(ExpenseModels.Request request)
        {
            var monthBeginDate = GetMonthBeginDate(request.BeginDate).ToUniversalTime();
            var monthEndDate = GetMonthEndDate(request.BeginDate).ToUniversalTime();

            var expenseTransactions = await _expenseRepo.GetExpensesBetweenTwoDatesPagination(monthBeginDate, monthEndDate, request.PageNumber, request.PageSize);
            var pagesAndExpenses = await GetTotalPages(request.PageSize, monthBeginDate, monthEndDate);
            var response = new ExpenseModels.Response
            {
                PageSize = request.PageSize,
                PageNumber = request.PageNumber,
                TotalPages = pagesAndExpenses.totalPages,
                TotalExpenses = pagesAndExpenses.totalExpenses,
                Expenses = _mapper.Map<ExpenseTransaction[]>(expenseTransactions)
            };
            return response;
        }
        internal DateTimeOffset GetMonthBeginDate(DateTimeOffset date)
        {
            var beginingOfMonth = new DateTimeOffset(
                date.Year, date.Month, 1, 0, 0, 0, new TimeSpan(0, 0, 0)
                );
            return beginingOfMonth;
        }
        internal DateTimeOffset GetMonthEndDate(DateTimeOffset date)
        {
            var monthEndDate = GetLastDayOfMonth(date);
            var endingOfMonth = new DateTimeOffset(
                date.Year, date.Month, monthEndDate, 0, 0, 0, new TimeSpan(0, 0, 0)
                );
            return endingOfMonth;
        }
        internal int GetLastDayOfMonth(DateTimeOffset date) => date.Month switch
        {
            4 or 6 or 9 or 11 => 30,
            1 or 3 or 5 or 7 or 8 or 10 or 12 => 31,
            2 => DateTime.IsLeapYear(date.Year) ? 29 : 28,
            _ => throw new ArgumentException($"Unable to determine the end of the month {date}", nameof(date.Month))
        };
        internal async Task<ExpenseModels.Response> GetExpensesForQuarterAsync(ExpenseModels.Request request)
        {
            var quarterDates = GetQuarterDatesFromDate(request.BeginDate);

            var expenses = await _expenseRepo.GetExpensesBetweenTwoDatesPagination(quarterDates.startDate, quarterDates.endDate, request.PageNumber, request.PageSize);
            var pagesAndExpenses = await GetTotalPages(request.PageSize, quarterDates.startDate, quarterDates.endDate);

            var response = new ExpenseModels.Response
            {
                PageSize = request.PageSize,
                PageNumber = request.PageNumber,
                TotalPages = pagesAndExpenses.totalPages,
                TotalExpenses = pagesAndExpenses.totalExpenses,
                Expenses = _mapper.Map<ExpenseTransaction[]>(expenses)
            };
            return response;
        }
        internal (DateTimeOffset startDate, DateTimeOffset endDate) GetQuarterDatesFromDate(DateTimeOffset date) => date.Month switch
        {
            1 or 2 or 3 => (startDate: new DateTimeOffset(date.Year, 1, 1, 0, 0, 0, new TimeSpan(0, 0, 0)), endDate: new DateTimeOffset(date.Year, 3, 31, 0, 0, 0, new TimeSpan(0, 0, 0))),
            4 or 5 or 6 => (startDate: new DateTimeOffset(date.Year, 4, 1, 0, 0, 0, new TimeSpan(0, 0, 0)), endDate: new DateTimeOffset(date.Year, 6, 30, 0, 0, 0, new TimeSpan(0, 0, 0))),
            7 or 8 or 9 => (startDate: new DateTimeOffset(date.Year, 7, 1, 0, 0, 0, new TimeSpan(0, 0, 0)), endDate: new DateTimeOffset(date.Year, 9, 30, 0, 0, 0, new TimeSpan(0, 0, 0))),
            10 or 11 or 12 => (startDate: new DateTimeOffset(date.Year, 10, 1, 0, 0, 0, new TimeSpan(0, 0, 0)), endDate: new DateTimeOffset(date.Year, 12, 31, 0, 0, 0, new TimeSpan(0, 0, 0))),
            _ => throw new ArgumentException($"Unable to determine quarter from given date {date}", nameof(date))
        };
        internal async Task<ExpenseModels.Response> GetExpensesForAYearAsync(ExpenseModels.Request request)
        {
            var yearDates = GetYearDatesFromDate(request.BeginDate);

            var expenseTransactions = await _expenseRepo.GetExpensesBetweenTwoDatesPagination(yearDates.startDate, yearDates.endDate, request.PageNumber, request.PageSize);
            var pagesAndExpenses = await GetTotalPages(request.PageSize, yearDates.startDate, yearDates.endDate);

            var response = new ExpenseModels.Response
            {
                PageSize = request.PageSize,
                PageNumber = request.PageNumber,
                TotalPages = pagesAndExpenses.totalPages,
                TotalExpenses = pagesAndExpenses.totalExpenses,
                Expenses = _mapper.Map<ExpenseTransaction[]>(expenseTransactions)
            };
            return response;
        }
        internal (DateTimeOffset startDate, DateTimeOffset endDate) GetYearDatesFromDate(DateTimeOffset date)
        {
            return (startDate: new DateTimeOffset(date.Year, 1, 1, 0, 0, 0, new TimeSpan(0, 0, 0)), endDate: new DateTimeOffset(date.Year, 12, 31, 0, 0, 0, new TimeSpan(0, 0, 0)));
        }
        internal async Task<ExpenseModels.Response> GetExpensesByDateRange(ExpenseModels.Request request)
        {
            var expenses = await _expenseRepo.GetExpensesBetweenTwoDatesPagination(request.BeginDate.ToUniversalTime(), request.EndDate.ToUniversalTime(), request.PageNumber, request.PageSize);
            var pagesAndExpenses = await GetTotalPages(request.PageSize, request.BeginDate.ToUniversalTime(), request.EndDate.ToUniversalTime());
            var response = new ExpenseModels.Response
            {
                PageSize = request.PageSize,
                PageNumber = request.PageNumber,
                TotalPages = pagesAndExpenses.totalPages,
                TotalExpenses = pagesAndExpenses.totalExpenses,
                Expenses = _mapper.Map<ExpenseTransaction[]>(expenses)
            };
            return response;
        }
        internal async Task<ExpenseModels.Response> GetExpensesFromLast30Days(ExpenseModels.Request request)
        {
            var thirtyDaysAgo = DateTimeOffset.UtcNow.AddDays(-30);
            var today = DateTimeOffset.UtcNow;

            var expenses = await _expenseRepo.GetExpensesBetweenTwoDatesPagination(thirtyDaysAgo, today, request.PageNumber, request.PageSize);
            var pagesAndExpenses = await GetTotalPages(request.PageSize, thirtyDaysAgo, today);

            var response = new ExpenseModels.Response
            {
                PageSize = request.PageSize,
                PageNumber = request.PageNumber,
                TotalPages = pagesAndExpenses.totalPages,
                TotalExpenses = pagesAndExpenses.totalExpenses,
                Expenses = _mapper.Map<ExpenseTransaction[]>(expenses)
            };
            return response;
        }
        internal async Task<ExpenseModels.Response> GetExpensesFromCurrentMonth(ExpenseModels.Request request)
        {
            var beginMonthDate = new DateTimeOffset(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0, new TimeSpan(0, 0, 0));
            var today = DateTimeOffset.UtcNow;

            var expenses = await _expenseRepo.GetExpensesBetweenTwoDatesPagination(beginMonthDate, today, request.PageNumber, request.PageSize);
            var pagesAndExpenses = await GetTotalPages(request.PageSize, beginMonthDate, today);

            var response = new ExpenseModels.Response
            {
                PageSize = request.PageSize,
                PageNumber = request.PageNumber,
                TotalPages = pagesAndExpenses.totalPages,
                TotalExpenses = pagesAndExpenses.totalExpenses,
                Expenses = _mapper.Map<ExpenseTransaction[]>(expenses)
            };
            return response;
        }
        internal async Task<ExpenseModels.Response> GetExpensesFromCurrentQuarter(ExpenseModels.Request request)
        {
            var beginQuarterDate = GetQuarterDatesFromDate(DateTimeOffset.UtcNow);
            var today = DateTimeOffset.UtcNow;

            var expenses = await _expenseRepo.GetExpensesBetweenTwoDatesPagination(beginQuarterDate.startDate, today, request.PageNumber, request.PageSize);
            var pagesAndExpenses = await GetTotalPages(request.PageSize, beginQuarterDate.startDate, today);

            var response = new ExpenseModels.Response
            {
                PageSize = request.PageSize,
                PageNumber = request.PageNumber,
                TotalPages = pagesAndExpenses.totalPages,
                TotalExpenses = pagesAndExpenses.totalExpenses,
                Expenses = _mapper.Map<ExpenseTransaction[]>(expenses)
            };
            return response;
        }
        internal async Task<ExpenseModels.Response> GetExpensesFromCurrentYear(ExpenseModels.Request request)
        {
            var beginYearDate = new DateTimeOffset(DateTime.Now.Year, 1, 1, 0, 0, 0, new TimeSpan(0, 0, 0));
            var today = DateTimeOffset.UtcNow;

            var expenses = await _expenseRepo.GetExpensesBetweenTwoDatesPagination(beginYearDate, today, request.PageNumber, request.PageSize);
            var pagesAndExpenses = await GetTotalPages(request.PageSize, beginYearDate, today);

            var response = new ExpenseModels.Response
            {
                PageSize = request.PageSize,
                PageNumber = request.PageNumber,
                TotalPages = pagesAndExpenses.totalPages,
                TotalExpenses = pagesAndExpenses.totalExpenses,
                Expenses = _mapper.Map<ExpenseTransaction[]>(expenses)
            };
            return response;
        }
        public async Task<Expenses> CreateUpdateExpenseAsync(AddEditExpense request)
        {
            var expense = _mapper.Map<Expenses>(request);

            var success = false;
            if (request.Id == null)
            {
                //I manually set the id here because when I use the test database it messes with the id autogeneration
                expense.id = ((int)await _expenseRepo.GetCountOfAllExpenses()) + 1;
                success = await _expenseRepo.CreateExpense(expense);
            }
            else
            {
                success = await _expenseRepo.UpdateExpense(expense);
            }

            if (!success)
                throw new Exception("Couldn't save expense to the database.");

            return expense;
        }
        public async Task<bool> DeleteExpenseAsync(int id)
        {
            var expense = await _expenseRepo.GetExpenseById(id);

            return await _expenseRepo.DeleteExpense(expense);
        }

        public async Task<ExpenseModels.Response> GetExpensesByNotesAsync(ExpenseModels.NotesSearchRequest request)
        {
            var expenses = await _expenseRepo.GetExpensesForNotesSearchPagination(request.SearchTerm, request.PageNumber, request.PageSize);
            var pagesAndExpenses = await GetCountOfExpensesForSearchingNotes(request.SearchTerm, request.PageSize);

            var response = new ExpenseModels.Response
            {
                PageSize = request.PageSize,
                PageNumber = request.PageNumber,
                TotalPages = pagesAndExpenses.totalPages,
                TotalExpenses = pagesAndExpenses.totalExpenses,
                Expenses = _mapper.Map<ExpenseTransaction[]>(expenses)
            };
            return response;
        }
        internal async Task<(int totalPages, int totalExpenses)> GetCountOfExpensesForSearchingNotes(string searchTerm, int pageSize)
        {
            decimal numberOfExpenses = await _expenseRepo.GetCountOfExpensesForNotesSearch(searchTerm);
            var totalPages = (int)Math.Ceiling(numberOfExpenses / (decimal)pageSize);
            return (totalPages == 0 ? 1 : totalPages, (int)numberOfExpenses);
        }

        public async Task<ExpenseModels.Response> GetExpensesByAmountAsync(ExpenseModels.AmountSearchRequest request)
        {
            var expenses = await _expenseRepo.GetExpensesForAmountSearchPagination(request.Query, request.PageNumber, request.PageSize);
            var pagesAndExpenses = await GetCountOfPagesForSearchingAmount(request.Query, request.PageSize);

            var response = new ExpenseModels.Response
            {
                PageSize = request.PageSize,
                PageNumber = request.PageNumber,
                TotalPages = pagesAndExpenses.totalPages,
                TotalExpenses = pagesAndExpenses.totalExpenses,
                Expenses = _mapper.Map<ExpenseTransaction[]>(expenses)
            };
            return response;
        }
        internal async Task<(int totalPages, int totalExpenses)> GetCountOfPagesForSearchingAmount(decimal amount, int pageSize)
        {
            decimal numberOfExpenses = await _expenseRepo.GetCountOfExpensesForAmountSearch(amount);
            var totalPages = (int)Math.Ceiling(numberOfExpenses / (decimal)pageSize);
            return (totalPages == 0 ? 1 : totalPages, (int)numberOfExpenses);
        }

        public async Task<ExpenseModels.Response> GetExpensesBySubCategoryAsync(ExpenseModels.SubCategorySearchRequest request)
        {
            var expenses = await _expenseRepo.GetExpensesForSubCategoryPagination(request.SubCategoryId, request.PageNumber, request.PageSize);
            var pagesAndExpenses = await GetCountOfPagesForSubCategory(request.SubCategoryId, request.PageSize);

            var response = new ExpenseModels.Response
            {
                PageSize = request.PageSize,
                PageNumber = request.PageNumber,
                TotalPages = pagesAndExpenses.totalPages,
                TotalExpenses = pagesAndExpenses.totalExpenses,
                Expenses = _mapper.Map<ExpenseTransaction[]>(expenses)
            };
            return response;
        }
        internal async Task<(int totalPages, int totalExpenses)> GetCountOfPagesForSubCategory(int subCategoryId, int pageSize)
        {
            decimal numberOfExpenses = await _expenseRepo.GetCountOfExpensesForSubCategoryAsync(subCategoryId);
            var totalPages = (int)Math.Ceiling(numberOfExpenses / (decimal)pageSize);
            return (totalPages == 0 ? 1 : totalPages, (int)numberOfExpenses);
        }
    }
}