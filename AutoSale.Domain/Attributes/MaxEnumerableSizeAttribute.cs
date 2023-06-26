using System.ComponentModel.DataAnnotations;

namespace AutoSale.Domain.Attributes
{
    public class MaxEnumerableSizeAttribute : ValidationAttribute
    {
        private readonly int _maxEnumerableSize;
        
        public MaxEnumerableSizeAttribute(int maxEnumerableSize)
        {
            _maxEnumerableSize = maxEnumerableSize;
        }

        public override bool IsValid(object? value)
        {
            var enumerable = value as IEnumerable<object?>;

            if (enumerable is not null)
            {
                return enumerable.Count() <= _maxEnumerableSize;
            }
            
            return true;
        }
    }
}