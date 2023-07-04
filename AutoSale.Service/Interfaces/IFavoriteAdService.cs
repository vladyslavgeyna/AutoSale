using AutoSale.Domain.Models;
using AutoSale.Domain.Response;

namespace AutoSale.Service.Interfaces
{
    public interface IFavoriteAdService
    {
        Task<IResponse<List<FavoriteAd>>> GetAllAsync(bool included = false);
        
        Task<IResponse<FavoriteAd>> GetByIdAsync(int id, bool included = false);
        
        Task<IResponse<FavoriteAd>> CreateAsync(FavoriteAd favoriteAd);
        
        Task<IResponse<List<FavoriteAd>>> GetByUserIdAsync(string userId, bool included = false);
        
        Task<IResponse<FavoriteAd>> GetByUserIdAndCarAdIdAsync(string userId, int carAdId, bool included = false);
        
        Task<IResponse<FavoriteAd>> EditAsync(FavoriteAd favoriteAd);
        
        Task<IResponse<bool>> RemoveAsync(int id);
        
        Task<IResponse<int>> GetCountOfFavoriteAdsByCarAdId(int carAdId);
    }
}