using CashTrack.Data.Entities;
using CashTrack.Repositories;
using CashTrack.Repositories.SubCategoriesRepository;

namespace CashTrack.Services.SubCategoryService;

public interface ISubCategoryService
{

}
public class SubCategoryService : ISubCategoryService
{
    private readonly ISubCategoryRepository _subCategoryRepo;

    public SubCategoryService(ISubCategoryRepository subCategoryRepo) => _subCategoryRepo = subCategoryRepo;

}

