using OdeToFood.Data.Models;
using OdeToFood.Data.Services;
using System.Collections.Generic;
using System.Web.Http;

namespace OdeToFood.Web.API
{
    public class RestaurantsController : ApiController
    {
        private readonly IRestaurantData _db;

        public RestaurantsController(IRestaurantData db)
        {
            _db = db;
        }

        public IEnumerable<Restaurant> Get()
        {
            var model = _db.GetAll();
            return model;
        }
    }
}
