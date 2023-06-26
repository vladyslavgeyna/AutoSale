using AutoSale.Domain.Models;
using AutoSale.Domain.Response;

namespace AutoSale.Service.Interfaces
{
    public interface ICarService
    {
        Task<IResponse<List<Car>>> GetAllAsync(bool included = false);
        
        Task<IResponse<Car>> GetByIdAsync(int id, bool included = false);
        
        Task<IResponse<Car>> CreateAsync(Car car);
        
        Task<IResponse<Car>> EditAsync(Car car);
        
        Task<IResponse<bool>> RemoveAsync(int id);
    }
}