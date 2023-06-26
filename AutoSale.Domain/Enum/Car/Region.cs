using System.ComponentModel.DataAnnotations;

namespace AutoSale.Domain.Enum.Car
{
    public enum Region
    {
        Cherkasy = 1,
        Chernihiv,
        Chernivtsi,
        Dnipropetrovsk,
        Donetsk,
        [Display(Name = "Ivano-Frankivsk")]
        IvanoFrankivsk,
        Kharkiv,
        Kherson,
        Khmelnytskyi,
        Kirovohrad,
        Kyiv,
        Luhansk,
        Lviv,
        Mykolaiv,
        Odesa,
        Poltava,
        Rivne,
        Sumy,
        Ternopil,
        Vinnytsia,
        Volyn,
        Zakarpattia,
        Zaporizhzhia,
        Zhytomyr,
    }
}