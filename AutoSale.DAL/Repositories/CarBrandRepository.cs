using AutoSale.DAL.Interfaces;
using AutoSale.Domain.Models;

namespace AutoSale.DAL.Repositories
{
    public class CarBrandRepository : ICarBrandRepository
    {
        private readonly ApplicationDbContext _context;

        public CarBrandRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<CarBrand> InsertAsync(CarBrand entity)
        {
            await _context.CarBrands.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public IQueryable<CarBrand> Select()
        {
            return _context.CarBrands;
        }

        public async Task DeleteAsync(CarBrand entity)
        {
            _context.CarBrands.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<CarBrand> UpdateAsync(CarBrand entity)
        {
            _context.CarBrands.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<CarBrand?> GetByIdAsync(int id)
        {
            return await _context.CarBrands.FindAsync(id);
        }
    }
}