using AutoSale.DAL.Interfaces;
using AutoSale.Domain.Models;

namespace AutoSale.DAL.Repositories
{
    public class UserReviewRepository : IUserReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public UserReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<UserReview> InsertAsync(UserReview entity)
        {
            await _context.UserReviews.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public IQueryable<UserReview> Select()
        {
            return _context.UserReviews;
        }

        public async Task DeleteAsync(UserReview entity)
        {
            _context.UserReviews.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<UserReview> UpdateAsync(UserReview entity)
        {
            _context.UserReviews.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<UserReview?> GetByIdAsync(int id)
        {
            return await _context.UserReviews.FindAsync(id);
        }
    }
}