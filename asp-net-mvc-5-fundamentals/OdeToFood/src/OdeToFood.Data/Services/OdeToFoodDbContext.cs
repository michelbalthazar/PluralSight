using OdeToFood.Data.Models;
using System.Data.Entity;

namespace OdeToFood.Data.Services
{
    public class OdeToFoodDbContext : DbContext
    {
        public OdeToFoodDbContext()
        {

        }

        public DbSet<Restaurant> Restaurants { get; set; }
    }
}