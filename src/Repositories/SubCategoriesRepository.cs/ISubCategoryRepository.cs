using CashTrack.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CashTrack.Repositories.SubCategoriesRepository.cs
{
    public interface ISubCategoryRepository
    {
        Task<List<ExpenseSubCategories>> GetAllSubCategoriesAsync();
    }
}
