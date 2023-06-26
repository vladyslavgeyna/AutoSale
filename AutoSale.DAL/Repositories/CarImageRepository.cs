using AutoSale.DAL.Interfaces;
using AutoSale.Domain.Models;

namespace AutoSale.DAL.Repositories
{
    public class CarImageRepository : ICarImageRepository
    {
        private readonly ApplicationDbContext _context;

        public CarImageRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<CarImage> InsertAsync(CarImage entity)
        {
            await _context.CarImages.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public IQueryable<CarImage> Select()
        {
            return _context.CarImages;
        }

        public async Task DeleteAsync(CarImage entity)
        {
            _context.CarImages.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<CarImage> UpdateAsync(CarImage entity)
        {
            _context.CarImages.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<CarImage?> GetByIdAsync(int id)
        {
            return await _context.CarImages.FindAsync(id);
        }
    }
}