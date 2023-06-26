using AutoSale.Domain.Models;
using AutoSale.Domain.Response;

namespace AutoSale.Service.Interfaces
{
    public interface ICarImageService
    {
        Task<IResponse<List<CarImage>>> GetAllAsync(bool included = false);
        
        Task<IResponse<CarImage>> GetByIdAsync(int id, bool included = false);
        
        Task<IResponse<List<CarImage>>> GetByCarIdAsync(int carId, bool included = false);
        
        Task<IResponse<CarImage>> CreateAsync(CarImage carImage);
        
        Task<IResponse<CarImage>> EditAsync(CarImage carImage);
        
        Task<IResponse<bool>> RemoveAsync(int id);
    }
}