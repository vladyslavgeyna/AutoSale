using AutoSale.DAL.Interfaces;
using AutoSale.Domain.Enum;
using AutoSale.Domain.Response;
using AutoSale.Service.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp.Formats.Jpeg;
using Image = AutoSale.Domain.Models.Image;

namespace AutoSale.Service.Implementations
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private const int CompressImageQuality = 40;
        public ImageService(IImageRepository imageRepository, IWebHostEnvironment webHostEnvironment)
        {
            _imageRepository = imageRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        
        public async Task<IResponse<List<Image>>> GetAllAsync()
        {
            try
            {
                var images = await _imageRepository.Select().ToListAsync();

                if (!images.Any())
                {
                    return new Response<List<Image>>
                    {
                        Data = images,
                        Description = $"Images not found",
                        Code = ResponseCode.NotFound
                    };
                }
                
                return new Response<List<Image>>
                {
                    Data = images,
                    Code = ResponseCode.Ok
                };
            }
            catch (Exception e)
            {
                return new Response<List<Image>>
                {
                    Description = $"[ImageService:GetAllAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }

        public async Task<IResponse<Image>> GetByIdAsync(int id)
        {
            try
            {
                var image = await _imageRepository.GetByIdAsync(id);

                if (image is null)
                {
                    return new Response<Image>
                    {
                        Description = $"Image not found",
                        Code = ResponseCode.NotFound
                    };
                }
                
                return new Response<Image>
                {
                    Data = image,
                    Code = ResponseCode.Ok
                };
            }
            catch (Exception e)
            {
                return new Response<Image>
                {
                    Description = $"[ImageService:GetByIdAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }

        private async Task<string> _createFileAndGetName(IFormFile formFile)
        {
            var wwwrootPath = _webHostEnvironment.WebRootPath;
            var extension = Path.GetExtension(formFile.FileName);
            string path, fileName;
            
            do
            {

                fileName = Guid.NewGuid().ToString() + extension;
                path = Path.Combine(wwwrootPath, "images", fileName);

            } while (File.Exists(path));

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await formFile.CopyToAsync(fileStream);
            }

            return fileName;
        }
        
        private async Task<string?> _createFileAndGetName(IFormFile formFile, int quality)
        {
            var wwwrootPath = _webHostEnvironment.WebRootPath;
            var extension = Path.GetExtension(formFile.FileName);
            string path, fileName;

            do
            {
                fileName = Guid.NewGuid().ToString() + extension;
                path = Path.Combine(wwwrootPath, "images", fileName);

            } while (File.Exists(path));

            using (var sourceImage = await SixLabors.ImageSharp.Image.LoadAsync(formFile.OpenReadStream()))
            {
                using (var outputStream = new MemoryStream())
                {
                    var encoder = new JpegEncoder
                    {
                        Quality = quality
                    };

                    await sourceImage.SaveAsync(outputStream, encoder);

                    await File.WriteAllBytesAsync(path, outputStream.ToArray());
                }
            }

            return fileName;
        }

        public async Task<IResponse<Image>> CreateAsync(IFormFile formFile)
        {
            try
            {
                var fileName = await _createFileAndGetName(formFile, CompressImageQuality);

                if (fileName is null)
                {
                    return new Response<Image>
                    {
                        Description = $"Unsupported image format",
                        Code = ResponseCode.NotFound
                    };
                }
                
                Image image = new()
                {
                    Name = fileName
                };

                image = await _imageRepository.InsertAsync(image);
                
                return new Response<Image>
                {
                    Data = image,
                    Code = ResponseCode.Ok
                };
            }
            catch (Exception e)
            {
                return new Response<Image>
                {
                    Description = $"[ImageService:CreateAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }

        public async Task<IResponse<Image>> EditAsync(int currentImageId, IFormFile newFormFile)
        {
            try
            {
                var currentImage = await _imageRepository.GetByIdAsync(currentImageId);
                
                if (currentImage is null)
                {
                    return new Response<Image>
                    {
                        Description = $"Current image not found",
                        Code = ResponseCode.NotFound
                    };
                }
                
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", currentImage.Name);
                
                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }

                var fileName = await _createFileAndGetName(newFormFile, CompressImageQuality);

                if (fileName is null)
                {
                    return new Response<Image>
                    {
                        Description = $"Unsupported image format",
                        Code = ResponseCode.NotFound
                    };
                }
                
                currentImage.Name = fileName;
                
                currentImage = await _imageRepository.UpdateAsync(currentImage);
                
                return new Response<Image>
                {
                    Data = currentImage,
                    Code = ResponseCode.Ok
                };
            }
            catch (Exception e)
            {
                return new Response<Image>
                {
                    Description = $"[ImageService:EditAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }

        public async Task<IResponse<bool>> RemoveAsync(int id)
        {
            try
            {
                var image = await _imageRepository.GetByIdAsync(id);
                
                if (image is null)
                {
                    return new Response<bool>
                    {
                        Data = false,
                        Description = $"Image not found",
                        Code = ResponseCode.NotFound
                    };
                }

                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", image.Name);

                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }

                await _imageRepository.DeleteAsync(image);
                
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
                    Description = $"[ImageService:RemoveAsync] - {e.Message}",
                    Code = ResponseCode.InternalServerError
                };
            }
        }

    }
}