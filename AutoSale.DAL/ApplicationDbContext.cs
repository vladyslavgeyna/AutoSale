using AutoSale.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AutoSale.DAL
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Car> Cars { get; set; }

        public DbSet<CarAd> CarAds { get; set; }

        public DbSet<CarBrand> CarBrands { get; set; }

        public DbSet<CarComparison> CarComparisons { get; set; }

        public DbSet<CarModel> CarModels { get; set; }

        public DbSet<Currency> Currencies { get; set; }

        public DbSet<FavoriteAd> FavoriteAds { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<UserReview> UserReviews { get; set; }

        public DbSet<CarImage> CarImages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}