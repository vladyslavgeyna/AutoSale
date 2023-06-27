using AutoSale.DAL.Interfaces;
using AutoSale.Domain.Models;

namespace AutoSale.DAL.Repositories
{
    public class FavoriteAdRepository : IFavoriteAdRepository
    {
        private readonly ApplicationDbContext _context;

        public FavoriteAdRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<FavoriteAd> InsertAsync(FavoriteAd entity)
        {
            await _context.FavoriteAds.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public IQueryable<FavoriteAd> Select()
        {
            return _context.FavoriteAds;
        }

        public async Task DeleteAsync(FavoriteAd entity)
        {
            _context.FavoriteAds.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<FavoriteAd> UpdateAsync(FavoriteAd entity)
        {
            _context.FavoriteAds.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<FavoriteAd?> GetByIdAsync(int id)
        {
            return await _context.FavoriteAds.FindAsync(id);
        }
    }
}