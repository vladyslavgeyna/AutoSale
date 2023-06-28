using AutoSale.DAL.Interfaces;
using AutoSale.Domain.Models;

namespace AutoSale.DAL.Repositories
{
    public class CarComparisionRepository : ICarComparisionRepository
    {
        private readonly ApplicationDbContext _context;

        public CarComparisionRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<CarComparison> InsertAsync(CarComparison entity)
        {
            await _context.CarComparisons.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public IQueryable<CarComparison> Select()
        {
            return _context.CarComparisons;
        }

        public async Task DeleteAsync(CarComparison entity)
        {
            _context.CarComparisons.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<CarComparison> UpdateAsync(CarComparison entity)
        {
            _context.CarComparisons.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<CarComparison?> GetByIdAsync(int id)
        {
            return await _context.CarComparisons.FindAsync(id);
        }
    }
}