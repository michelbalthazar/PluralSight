using OdeToFood.Data.Models;
using OdeToFood.Data.Services;
using System.Web.Mvc;

namespace OdeToFood.Web.Controllers
{
    public class RestaurantsController : Controller
    {
        private readonly IRestaurantData _db;

        public RestaurantsController(IRestaurantData db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = _db.GetAll();
            return View(model);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var model = _db.GetOne(id);

            if (model == null)
                return View("NotFound");

            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                _db.Add(restaurant);
                return RedirectToAction("Details", new { id = restaurant.Id });
            }

            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = _db.GetOne(id);

            if (model == null)
                return View("NotFound");
            //return HttpNotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                _db.Update(restaurant);
                return RedirectToAction("Details", new { id = restaurant.Id });
            }

            return View(restaurant);
        }
    }
}