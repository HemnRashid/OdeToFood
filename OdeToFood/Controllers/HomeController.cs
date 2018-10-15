
using Microsoft.AspNetCore.Mvc;
using OdeToFood.Models;
using OdeToFood.Services;
using OdeToFood.ViewModels;
using System;

namespace OdeToFood.Controllers
{

    
    /// <summary>
    /// this name is significant, becuase the MVC framework has some very specific convensions about how it map an incoming request to a method on a class.
    /// by default, it is the HomeController that will receive a request to the root of the application.
    /// </summary>
    public class HomeController : Controller
    {
        private IRestaurantData _restaurantData;
        private IGreeter _greeter;


        /// <summary>
        /// Här används Dependency Injuction
        /// </summary>
        /// <param name="restaurantData"></param>
        public HomeController(IRestaurantData restaurantData,IGreeter greeter)
        {
            _restaurantData = restaurantData;
            _greeter = greeter;
        }


        /// <summary>
        /// Default method that Homecontroller checks
        /// </summary>
        /// <returns>string</returns>
        public IActionResult Index()
        {

            // 1. var model = new Restaurant { Id = 1, Name = "Kurdistan" };

            // 2. var model = _restaurantData.GetAll();

            // 3.
            var model = new HomeIndexViewModel();
            model.Restaurants = _restaurantData.GetAll();
            model.CurrentMessage = _greeter.GetMessageOfTheDay();
           
            // utan overload, så returneras svaret till en sida som heter index, samma namn som metoden.
            // dock måste Home existera i /View/Home/home.cshtml för att få den att fungera med en kontroller eller om den ska delas med flera kontroller måste den Home läggas i /View/shered/Home.cshtml
            //return View("Home");  

            /*return View();*/ // diregeras till Index.cshtml

            return View(model);



            // resultatet skickas genom ObjectResult.. 
            //return new ObjectResult(model);  // serialiserar till en viss format.. men default är json, beror på accept header i http response vad för typ av format den ska leverera tillbaka svaret.

            //return this.BadRequest();

            //return this.Content("Hello from the HomeController");

            //return "Hello from the HomeController";
        }


        public IActionResult Details(int id)
        {
            var model = _restaurantData.Get(id);
            if(model==null)
            {

                //return NotFound();  // används for API. 
                
                return RedirectToAction(nameof(Index));

            }


            return View(model);
            

            //return Content(id.ToString());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RestaurantEditModel model)
        {
            if (ModelState.IsValid)
            {
                var newRestaurant = new Restaurant();
                newRestaurant.Name = model.Name;
                newRestaurant.Cuisine = model.Cuisine;

                newRestaurant = _restaurantData.Add(newRestaurant);
                return RedirectToAction(nameof(Details), new { id = newRestaurant.Id }); // använder redirect för att undvika att skapa objectet på nytt.
            }
            else
            {
                return View();
            }
            //return View("Details",newRestaurant);
        }

        [HttpPost]
        public IActionResult Edit(int id)
        {
            var model = _restaurantData.Get(id);
            if(model==null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


    }
}
