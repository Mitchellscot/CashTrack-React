using CashTrack.Data.Entities;
using CashTrack.Models.Expenses;
using System.Threading.Tasks;

namespace CashTrack.Services.TagRepository
{
    public interface ITagService
    {
        Task<Tags[]> GetAllTags();
    }
}
