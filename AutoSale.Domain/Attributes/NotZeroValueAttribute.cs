using System.ComponentModel.DataAnnotations;

namespace AutoSale.Domain.Attributes
{
    public class NotZeroValueAttribute : ValidationAttribute
    {

        public override bool IsValid(object? value)
        {
            try
            {
                var item = (int)value;

                return item != 0;
            }
            catch (Exception e)
            {
                return true;
            }
        }
    }
}