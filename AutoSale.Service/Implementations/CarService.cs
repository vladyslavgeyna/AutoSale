using AutoSale.DAL.Interfaces;
using AutoSale.Domain.Enum;
using AutoSale.Domain.Models;
using AutoSale.Domain.Response;
using AutoSale.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AutoSale.Service.Implementations
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;

        public CarService(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }
        
        public async Task<IResponse<List<Car>>> GetAllAsync(bool included = false)
        {
            try
            {
                var cars = included
                    ? await _carRepository.Select()
                        .Include(c => c.CarBrand)
                        .Include(c => c.CarModel)
                        .Include(c => c.Currency)
                        .ToListAsync()
                    : await _carRepository.Select().ToListAsync();
                
                if (!cars.Any())
                {
                    return new Response<List<Car>>
                    {
                        Data = cars,
                        Description = $"Cars not found",
                        Code = ResponseCode.NotFound
                    };
                }
                
                return new Response<List<Car>>
                {
                    Data = cars,
                    Code = ResponseCode.Ok
                };
            }
            catch (Exception e)
            {
                return new Response<List<Car>>
                {
                    Description = $"[CarService:GetAllAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }

        public async Task<IResponse<Car>> GetByIdAsync(int id, bool included = false)
        {
            try
            {
                var car = included
                    ? await _carRepository.Select()
                        .Include(c => c.CarBrand)
                        .Include(c => c.CarModel)
                        .Include(c => c.Currency)
                        .FirstOrDefaultAsync(c => c.Id == id)
                    : await _carRepository.GetByIdAsync(id);

                if (car is null)
                {
                    return new Response<Car>
                    {
                        Description = $"Car not found",
                        Code = ResponseCode.NotFound
                    };
                }
                
                return new Response<Car>
                {
                    Data = car,
                    Code = ResponseCode.Ok
                };
            }
            catch (Exception e)
            {
                return new Response<Car>
                {
                    Description = $"[CarService:GetByIdAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }

        public async Task<IResponse<Car>> CreateAsync(Car car)
        {
            try
            {
                car = await _carRepository.InsertAsync(car);
                
                return new Response<Car>
                {
                    Data = car,
                    Code = ResponseCode.Ok
                };
            }
            catch (Exception e)
            {
                return new Response<Car>
                {
                    Description = $"[CarService:CreateAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }

        public async Task<IResponse<Car>> EditAsync(Car car)
        {
            try
            {
                car = await _carRepository.UpdateAsync(car);
                
                return new Response<Car>
                {
                    Data = car,
                    Code = ResponseCode.Ok
                };
            }
            catch (Exception e)
            {
                return new Response<Car>
                {
                    Description = $"[CarService:EditAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }        
        }

        public async Task<IResponse<bool>> RemoveAsync(int id)
        {
            try
            {
                var car = await _carRepository.GetByIdAsync(id);
                
                if (car is null)
                {
                    return new Response<bool>
                    {
                        Data = false,
                        Description = $"Car not found",
                        Code = ResponseCode.NotFound
                    };
                }

                await _carRepository.DeleteAsync(car);
                
                return new Response<bool>
                {
                    Data = true,
                    Code = ResponseCode.Ok
                };
            }
            catch (Exception e)
            {
                return new Response<bool>
                {
                    Data = false,
                    Description = $"[CarService:RemoveAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }
    }
}