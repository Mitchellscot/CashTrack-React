using CashTrack.Data;
using CashTrack.Data.Entities;
using CashTrack.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace CashTrack.Repositories.TagRepository
{
    public class TagRepository : ITagRepository
    {
        private readonly AppSettings _appSettings;
        private readonly AppDbContext _context;
        private readonly ILogger<TagRepository> _logger;

        public TagRepository(
            IOptions<AppSettings> appSettings, AppDbContext context, ILogger<TagRepository> logger)
        {
            this._appSettings = appSettings.Value;
            this._context = context;
            this._logger = logger;
        }
        public async Task<Tags[]> GetAllTags()
        {
            var tags = await _context.Tags.ToArrayAsync();
            return tags;
        }
    }
}
