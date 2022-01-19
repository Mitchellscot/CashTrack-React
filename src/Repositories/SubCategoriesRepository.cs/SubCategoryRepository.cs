using CashTrack.Data;
using CashTrack.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CashTrack.Repositories.SubCategoriesRepository.cs
{
    public class SubCategoryRepository : ISubCategoryRepository
    {
        private readonly AppDbContext _context;
        public SubCategoryRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<ExpenseSubCategories>> GetAllSubCategoriesAsync()
        {
            return await _context.ExpenseSubCategories.ToListAsync();
        }
    }
}
