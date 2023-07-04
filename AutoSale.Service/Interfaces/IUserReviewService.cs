using AutoSale.Domain.Models;
using AutoSale.Domain.Response;

namespace AutoSale.Service.Interfaces
{
    public interface IUserReviewService
    {
        Task<IResponse<List<UserReview>>> GetAllAsync(bool included = false);
        
        Task<IResponse<UserReview>> GetByIdAsync(int id, bool included = false);
        
        Task<IResponse<UserReview>> CreateAsync(UserReview userReview);
        
        Task<IResponse<List<UserReview>>> GetByUserIdToAsync(string userIdTo, bool included = false);
        
        Task<IResponse<UserReview>> EditAsync(UserReview userReview);
        
        Task<IResponse<bool>> RemoveAsync(int id);
    }
}