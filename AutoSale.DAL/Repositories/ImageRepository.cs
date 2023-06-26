using AutoSale.DAL.Interfaces;
using AutoSale.Domain.Models;

namespace AutoSale.DAL.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly ApplicationDbContext _context;

        public ImageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Image> InsertAsync(Image entity)
        {
            await _context.Images.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public IQueryable<Image> Select()
        {
            return _context.Images;
        }

        public async Task DeleteAsync(Image entity)
        {
            _context.Images.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<Image> UpdateAsync(Image entity)
        {
            _context.Images.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Image?> GetByIdAsync(int id)
        {
            return await _context.Images.FindAsync(id);
        }
    }
}