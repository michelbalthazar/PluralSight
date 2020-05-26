﻿using OdeToFood.Data.Models;
using System.Collections.Generic;

namespace OdeToFood.Data.Services
{
    public interface IRestaurantData
    {
        IEnumerable<Restaurant> GetAll();
        Restaurant GetOne(int id);
        void Add(Restaurant restaurant);
    }
}