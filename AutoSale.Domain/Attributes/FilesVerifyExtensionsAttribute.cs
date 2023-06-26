using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AutoSale.Domain.Attributes
{
    public class FilesVerifyExtensionsAttribute : ValidationAttribute
    {
        private List<string> _allowedExtensions { get; set; }

        public FilesVerifyExtensionsAttribute(string fileExtensions)
        {
            _allowedExtensions = fileExtensions.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
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
                        var fileName = file.FileName;
                        if (!_allowedExtensions.Any(y => fileName.EndsWith(y)))
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