using AutoSale.DAL.Interfaces;
using AutoSale.Domain.Models;

namespace AutoSale.DAL.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly ApplicationDbContext _context;

        public CurrencyRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<Currency> InsertAsync(Currency entity)
        {
            await _context.Currencies.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public IQueryable<Currency> Select()
        {
            return _context.Currencies;
        }

        public async Task DeleteAsync(Currency entity)
        {
            _context.Currencies.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<Currency> UpdateAsync(Currency entity)
        {
            _context.Currencies.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Currency?> GetByIdAsync(int id)
        {
            return await _context.Currencies.FindAsync(id);
        }
    }
}