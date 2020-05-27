using OdeToFood.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace OdeToFood.Data.Services
{
    public class SqlRestaurantData : IRestaurantData
    {
        private readonly OdeToFoodDbContext _db;

        public SqlRestaurantData(OdeToFoodDbContext db)
        {
            _db = db;
        }

        public void Add(Restaurant restaurant)
        {
            _db.Restaurants.Add(restaurant);
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = GetOne(id);

            _db.Restaurants.Remove(entity);
            _db.SaveChanges();
        }

        public IEnumerable<Restaurant> GetAll()
        {
            return from e in _db.Restaurants
                   orderby e.Name
                   select e;
        }

        public Restaurant GetOne(int id)
        {
            return _db.Restaurants.FirstOrDefault(e => e.Id == id);
        }

        public void Update(Restaurant restaurant)
        {
            var entry = _db.Entry(restaurant);
            entry.State = EntityState.Modified;
            _db.SaveChanges();
        }
    }
}