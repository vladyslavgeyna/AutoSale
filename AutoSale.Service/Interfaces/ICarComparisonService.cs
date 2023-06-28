using AutoSale.Domain.Models;
using AutoSale.Domain.Response;

namespace AutoSale.Service.Interfaces
{
    public interface ICarComparisonService
    {
        Task<IResponse<List<CarComparison>>> GetAllAsync(bool included = false);
        
        Task<IResponse<CarComparison>> GetByIdAsync(int id, bool included = false);
        
        Task<IResponse<CarComparison>> CreateAsync(CarComparison carComparison);
        
        Task<IResponse<List<CarComparison>>> GetByUserIdAsync(string userId, bool included = false);
        
        Task<IResponse<CarComparison>> GetByUserIdAndCarAdIdAsync(string userId, int carAdId, bool included = false);
        
        Task<IResponse<CarComparison>> EditAsync(CarComparison carComparison);
        
        Task<IResponse<bool>> RemoveAsync(int id);
    }
}