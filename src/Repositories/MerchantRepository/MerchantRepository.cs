using CashTrack.Data;
using CashTrack.Data.Entities;
using CashTrack.Helpers.Aggregators;
using CashTrack.Helpers.Exceptions;
using CashTrack.Models.ExpenseModels;
using CashTrack.Models.MerchantModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

        public async Task<Expenses[]> GetExpensesAndCategoriesByMerchantIdAsync(int id)
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

        public async Task<Merchants> GetMerchantByIdAsync(int id)
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

        public async Task<Merchants[]> GetMerchantsPaginationAsync(int pageSize, int pageNumber)
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

        public async Task<Merchants[]> GetMerchantsBySearchTermAsync(string searchTerm, int pageSize, int pageNumber)
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

        public async Task<Merchants[]> GetAllMerchantsAsync()
        {
            try
            {
                return await _context.Merchants.ToArrayAsync();
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
    }
}