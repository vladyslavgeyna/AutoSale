using AutoSale.DAL.Interfaces;
using AutoSale.Domain.Models;

namespace AutoSale.DAL.Repositories
{
    public class CarAdRepository : ICarAdRepository
    {
        private readonly ApplicationDbContext _context;

        public CarAdRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<CarAd> InsertAsync(CarAd entity)
        {
            await _context.CarAds.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public IQueryable<CarAd> Select()
        {
            return _context.CarAds;
        }

        public async Task DeleteAsync(CarAd entity)
        {
            _context.CarAds.Remove(entity);
            await _context.SaveChangesAsync();        
        }

        public async Task<CarAd> UpdateAsync(CarAd entity)
        {
            _context.CarAds.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<CarAd?> GetByIdAsync(int id)
        {
            return await _context.CarAds.FindAsync(id);
        }
    }
}