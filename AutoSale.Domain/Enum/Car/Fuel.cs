using System.ComponentModel.DataAnnotations;

namespace AutoSale.Domain.Enum.Car
{
    public enum Fuel
    {
        Diesel = 1,
        Gas,
        Gasoline,
        [Display(Name = "Gas and gasoline")]
        GasAndGasoline,
        Electric,
    }
}