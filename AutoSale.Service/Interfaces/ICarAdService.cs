using AutoSale.Domain.Models;
using AutoSale.Domain.Response;
using AutoSale.Domain.ViewModels.CarAd;

namespace AutoSale.Service.Interfaces
{
    public interface ICarAdService
    {
        Task<IResponse<List<CarAd>>> GetAllAsync(bool included = false);
        
        Task<IResponse<CarAd>> GetByIdAsync(int id, bool included = false);
        
        Task<IResponse<List<CarAd>>> GetByUserIdAsync(string userId, bool included = false);
        
        Task<IResponse<CarAd>> CreateAsync(CreateCarAdViewModel createCarAdViewModel);
        
        Task<IResponse<CarAd>> EditAsync(CarAd carAd);
        
        Task<IResponse<bool>> RemoveAsync(int id);
    }
}