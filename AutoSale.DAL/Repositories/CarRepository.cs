using AutoSale.DAL.Interfaces;
using AutoSale.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoSale.DAL.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly ApplicationDbContext _context;

        public CarRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<Car> InsertAsync(Car entity)
        {
            await _context.Cars.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public IQueryable<Car> Select()
        {
            return _context.Cars;
        }

        public async Task DeleteAsync(Car entity)
        {
            _context.Cars.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<Car> UpdateAsync(Car entity)
        {
            _context.Cars.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Car?> GetByIdAsync(int id)
        {
            return await _context.Cars.FindAsync(id);
        }
    }
}