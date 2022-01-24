using AutoMapper;
using CashTrack.Data.Entities;
using CashTrack.Helpers.Exceptions;
using CashTrack.Models.MainCategoryModels;
using CashTrack.Repositories.MainCategoriesRepository;
using System.Linq;
using System.Threading.Tasks;

namespace CashTrack.Services.MainCategoriesService
{
    public interface IMainCategoriesService
    {
        Task<MainCategoryModels.Response> GetMainCategoriesAsync(MainCategoryModels.Request request);
        Task<MainCategoryDetail> GetMainCategoryDetailAsync(int id);
        Task<AddEditMainCategory> CreateMainCategory(AddEditMainCategory request);
        Task<bool> UpdateMainCategoryAsync(AddEditMainCategory newMainCategory);
        Task<bool> DeleteMainCategoryAsync(int id);
    }
    public class MainCategoriesService : IMainCategoriesService
    {
        private readonly IMainCategoriesRepository _repo;
        private readonly IMapper _mapper;

        public MainCategoriesService(IMainCategoriesRepository mainCategoryRepository, IMapper mapper)
        {
            _repo = mainCategoryRepository;
            _mapper = mapper;
        }

        public async Task<MainCategories> CreateMainCategory(AddEditMainCategory request)
        {
            var categories = await _repo.Find(x => true);
            if (categories.Any(x => x.main_category_name == request.Name))
                throw new DuplicateCategoryNameException(request.Name);

            var category = _mapper.Map<MainCategories>(request);

            category.id = (int)await _repo.GetCountOfMainCategories() + 1;

            if (!await _repo.Create(category))
                throw new System.Exception("unable to save category to the database");

            return category;
        }

        public Task<bool> DeleteMainCategoryAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<MainCategoryModels.Response> GetMainCategoriesAsync(MainCategoryModels.Request request)
        {
            throw new System.NotImplementedException();
        }

        public Task<MainCategoryDetail> GetMainCategoryDetailAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> UpdateMainCategoryAsync(AddEditMainCategory newMainCategory)
        {
            throw new System.NotImplementedException();
        }
    }
}
