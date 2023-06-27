using AutoSale.DAL.Interfaces;
using AutoSale.Domain.Enum;
using AutoSale.Domain.Models;
using AutoSale.Domain.Response;
using AutoSale.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AutoSale.Service.Implementations
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;

        public CurrencyService(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }
        
        public async Task<IResponse<List<Currency>>> GetAllAsync()
        {
            try
            {
                var currencies = await _currencyRepository.Select().ToListAsync();
                
                if (!currencies.Any())
                {
                    return new Response<List<Currency>>
                    {
                        Data = currencies,
                        Description = $"Currencies not found",
                        Code = ResponseCode.NotFound
                    };
                }
                
                return new Response<List<Currency>>
                {
                    Data = currencies,
                    Code = ResponseCode.Ok
                };
            }
            catch (Exception e)
            {
                return new Response<List<Currency>>
                {
                    Description = $"[CurrencyService:GetAllAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }

        public async Task<IResponse<Currency>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IResponse<Currency>> CreateAsync(Currency currency)
        {
            throw new NotImplementedException();
        }

        public async Task<IResponse<Currency>> EditAsync(Currency currency)
        {
            throw new NotImplementedException();
        }

        public async Task<IResponse<Currency>> RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}