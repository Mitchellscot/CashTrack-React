using CashTrack.Data.Entities;
using CashTrack.Models.ExpenseModels;
using System.Threading.Tasks;

namespace CashTrack.Repositories.TagRepository
{
    public interface ITagRepository
    {
        Task<Tags[]> GetAllTags();
    }
}
