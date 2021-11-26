using CashTrack.Data.Entities;
using CashTrack.Models.ExpenseModels;
using System.Threading.Tasks;

namespace CashTrack.Services.TagRepository
{
    public interface ITagService
    {
        Task<Tags[]> GetAllTags();
    }
}
