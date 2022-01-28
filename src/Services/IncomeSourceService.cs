using CashTrack.Repositories.IncomeSourceRepository;

namespace CashTrack.Services.IncomeSourceService;

public interface IIncomeSourceService
{

}
public class IncomeSourceService : IIncomeSourceService
{
    private readonly IIncomeSourceRepository _repo;

    public IncomeSourceService(IIncomeSourceRepository repo) => _repo = repo;


}

