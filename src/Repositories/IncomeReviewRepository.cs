using CashTrack.Data;
using CashTrack.Data.Entities;

namespace CashTrack.Repositories.IncomeReviewRepository;

public class IncomeReviewRepository : TransactionRepository<IncomeReview>
{
    public IncomeReviewRepository(AppDbContext ctx) : base(ctx)
    {
    }
}

