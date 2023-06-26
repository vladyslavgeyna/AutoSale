using AutoSale.Domain.Models;
using AutoSale.Domain.Response;

namespace AutoSale.Service.Interfaces
{
    public interface ICurrencyService
    {
        Task<IResponse<List<Currency>>> GetAllAsync();
        
        Task<IResponse<Currency>> GetByIdAsync(int id);
        
        Task<IResponse<Currency>> CreateAsync(Currency currency);
        
        Task<IResponse<Currency>> EditAsync(Currency currency);
        
        Task<IResponse<Currency>> RemoveAsync(int id);
    }
}