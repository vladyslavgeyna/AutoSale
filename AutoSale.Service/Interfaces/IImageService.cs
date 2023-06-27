using AutoSale.Domain.Models;
using AutoSale.Domain.Response;
using Microsoft.AspNetCore.Http;
using Image = AutoSale.Domain.Models.Image;

namespace AutoSale.Service.Interfaces
{
    public interface IImageService
    {
        Task<IResponse<List<Image>>> GetAllAsync();
        
        Task<IResponse<Image>> GetByIdAsync(int id);
        
        Task<IResponse<Image>> CreateAsync(IFormFile formFile);
        
        Task<IResponse<Image>> EditAsync(int currentImageId, IFormFile newFormFile);
        
        Task<IResponse<bool>> RemoveAsync(int id);

    }
}