using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AutoSale.Domain.Attributes
{
    public class FileVerifyExtensionsAttribute : ValidationAttribute
    {
        private List<string> _allowedExtensions { get; set; }

        public FileVerifyExtensionsAttribute(string fileExtensions)
        {
            _allowedExtensions = fileExtensions.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public override bool IsValid(object? value)
        {
            var file = value as IFormFile;

            if (file is not null)
            {
                var fileName = file.FileName;
                return _allowedExtensions.Any(y => fileName.EndsWith(y));
            }

            return true;
        }
    }
}