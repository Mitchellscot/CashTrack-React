using CashTrack.Data;
using CashTrack.Data.Entities;
using CashTrack.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace CashTrack.Services.TagRepository
{
    public class TagService : ITagService
    {
        private readonly AppSettings _appSettings;
        private readonly AppDbContext _context;
        private readonly ILogger<TagService> _logger;

        public TagService(
            IOptions<AppSettings> appSettings, AppDbContext context, ILogger<TagService> logger)
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
