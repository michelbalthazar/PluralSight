using OdeToFood.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace OdeToFood.Data.Services
{
    public class InMemoryRestaurantData : IRestaurantData
    {
        List<Restaurant> restaurants;

        public InMemoryRestaurantData()
        {
            restaurants = new List<Restaurant>
            {
                new Restaurant { Id = 1, Name ="Scott's Pizza", Cuisine= CuisineType.Italian },
                new Restaurant { Id = 2, Name ="Tersiguels", Cuisine= CuisineType.French },
                new Restaurant { Id = 3, Name ="Mango Grove", Cuisine= CuisineType.Indian }
            };
        }

        public void Add(Restaurant restaurant)
        {
            restaurant.Id = restaurants.Max(e => e.Id) + 1;
            restaurants.Add(restaurant);
        }

        public IEnumerable<Restaurant> GetAll()
        {
            return restaurants.OrderBy(e => e.Name);
        }

        public Restaurant GetOne(int id)
        {
            return restaurants.FirstOrDefault(e => e.Id == id);

        }

        public void Update(Restaurant restaurant)
        {
            var existing = GetOne(restaurant.Id);
            if (existing != null)
            {
                existing.Name = restaurant.Name;
                existing.Cuisine = restaurant.Cuisine;
            }
            //restaurants.RemoveAll(e => e.Id == restaurant.Id);
            //restaurants.Add(restaurant);
        }
    }
}