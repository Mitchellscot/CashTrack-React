using CashTrack.Data;
using CashTrack.Data.Entities;
using CashTrack.Helpers.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CashTrack.Repositories.MerchantRepository
{
    public class MerchantRepository : IMerchantRepository
    {
        private readonly AppDbContext _context;

        public MerchantRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Expenses[]> GetExpensesAndCategoriesByMerchantId(int id)
        {
            try
            {
                var expenses = await _context.Expenses.Where(e => e.merchant.id == id).Include(x => x.category).ToArrayAsync();
                return expenses;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Merchants> GetMerchantById(int id)
        {
            try
            {
                var merchant = await _context.Merchants.FindAsync(id);
                if (merchant == null)
                    throw new MerchantNotFoundException(id.ToString());

                return merchant;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Merchants[]> GetMerchantsPagination(int pageSize, int pageNumber)
        {
            try
            {
                var merchants = await _context.Merchants
                    .OrderBy(x => x.name)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToArrayAsync();
                return merchants;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<Merchants[]> GetMerchantsPaginationSearchTerm(string searchTerm, int pageSize, int pageNumber)
        {
            try
            {
                var merchants = await _context.Merchants
                    .Where(x => x.name.ToLower().Contains(searchTerm.ToLower()))
                    .OrderBy(x => x.name)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToArrayAsync();
                return merchants;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> GetNumberOfExpensesForMerchant(int id)
        {
            try
            {
                return await _context.Expenses.CountAsync(x => x.merchant.id == id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Merchants[]> GetAllMerchantsNoTracking()
        {
            try
            {
                return await _context.Merchants.AsNoTracking().ToArrayAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> CreateMerchant(Merchants merchant)
        {
            try
            {
                await _context.Merchants.AddAsync(merchant);
                return await (_context.SaveChangesAsync()) > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> UpdateMerchant(Merchants merchant)
        {
            try
            {
                var entity = _context.Merchants.Attach(merchant);
                entity.State = EntityState.Modified;
                return await (_context.SaveChangesAsync()) > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteMerchant(Merchants merchant)
        {
            try
            {
                _context.Remove(merchant);
                return await (_context.SaveChangesAsync()) > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<decimal> GetCountOfAllMerchants()
        {
            try
            {
                return (decimal)await _context.Merchants.CountAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<decimal> GetCountOfAllMerchantsSearch(string searchTerm)
        {
            try
            {
                return (decimal)await _context.Merchants.Where(x => x.name.Contains(searchTerm)).CountAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}