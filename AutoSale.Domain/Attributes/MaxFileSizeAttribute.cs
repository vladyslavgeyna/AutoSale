using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AutoSale.Domain.Attributes
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSizeInBytes;
        
        public MaxFileSizeAttribute(int maxFileSizeInBytes)
        {
            _maxFileSizeInBytes = maxFileSizeInBytes;
        }

        public override bool IsValid(object? value)
        {
            var file = value as IFormFile;
            
            if (file is not null)
            {
                return file.Length <= _maxFileSizeInBytes;
            }

            return true;
        }

    }
}