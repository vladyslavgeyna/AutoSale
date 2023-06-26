using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AutoSale.Domain.Attributes
{
    public class MaxFilesSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSizeInBytes;
        
        public MaxFilesSizeAttribute(int maxFileSizeInBytes)
        {
            _maxFileSizeInBytes = maxFileSizeInBytes;
        }

        public override bool IsValid(object? value)
        {
            var files = value as IList<IFormFile?>;

            if (files is not null)
            {
                foreach (var file in files)
                {
                    if (file is not null)
                    {
                        if (file.Length > _maxFileSizeInBytes)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            
            return true;
        }
    }
}