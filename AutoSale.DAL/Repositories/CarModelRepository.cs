using AutoSale.DAL.Interfaces;
using AutoSale.Domain.Models;

namespace AutoSale.DAL.Repositories
{
    public class CarModelRepository : ICarModelRepository
    {
        private readonly ApplicationDbContext _context;

        public CarModelRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<CarModel> InsertAsync(CarModel entity)
        {
            await _context.CarModels.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public IQueryable<CarModel> Select()
        {
            return _context.CarModels;
        }

        public async Task DeleteAsync(CarModel entity)
        {
            _context.CarModels.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<CarModel> UpdateAsync(CarModel entity)
        {
            _context.CarModels.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<CarModel?> GetByIdAsync(int id)
        {
            return await _context.CarModels.FindAsync(id);
        }
    }
}