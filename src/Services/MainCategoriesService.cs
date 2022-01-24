using CashTrack.Data.Entities;
using CashTrack.Repositories;
using CashTrack.Repositories.MainCategoriesRepository;

namespace CashTrack.Services.MainCategoriesService
{
    public interface IMainCategoriesService
    {

    }
    public class MainCategoriesService : IMainCategoriesService
    {
        private readonly IMainCategoriesRepository _repo;

        public MainCategoriesService(IMainCategoriesRepository mainCategoryRepository)
        {
            _repo = mainCategoryRepository;
        }
    }
}
