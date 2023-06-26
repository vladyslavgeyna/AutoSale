using System.ComponentModel.DataAnnotations;

namespace AutoSale.Domain.Enum.Car
{
    public enum Transmission
    {
        Manual = 1,
        Automatic,
        CVT,
        [Display(Name = "Dual clutch")]
        DualClutch,
    }
}