using AutoSale.DAL.Interfaces;
using AutoSale.DAL.Repositories;
using AutoSale.Service.Implementations;
using AutoSale.Service.Interfaces;

namespace AutoSaleMVC
{
    public static class Initializer
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<ICarRepository, CarRepository>();
            services.AddScoped<ICarAdRepository, CarAdRepository>();
            services.AddScoped<ICarImageRepository, CarImageRepository>();
            services.AddScoped<ICarBrandRepository, CarBrandRepository>();
            services.AddScoped<ICarModelRepository, CarModelRepository>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<IFavoriteAdRepository, FavoriteAdRepository>();
            services.AddScoped<ICarComparisionRepository, CarComparisionRepository>();
        }

        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<ICarService, CarService>();
            services.AddScoped<ICarAdService, CarAdService>();
            services.AddScoped<ICarImageService, CarImageService>();
            services.AddScoped<ICarBrandService, CarBrandService>();
            services.AddScoped<ICarModelService, CarModelService>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IFavoriteAdService, FavoriteAdService>();
            services.AddScoped<ICarComparisonService, CarComparisonService>();
        }
    }
}