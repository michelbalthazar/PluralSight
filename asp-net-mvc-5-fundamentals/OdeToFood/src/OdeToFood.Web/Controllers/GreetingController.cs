﻿using OdeToFood.Web.Models;
using System.Configuration;
using System.Web.Mvc;

namespace OdeToFood.Web.Controllers
{
    public class GreetingController : Controller
    {
        // GET: Greeting
        public ActionResult Index(string name)
        {
            var model = new GreetingViewModel
            {
                Message = ConfigurationManager.AppSettings["Message"],
                Name = name ?? "no name"
            };

            return View(model);
        }
    }
}