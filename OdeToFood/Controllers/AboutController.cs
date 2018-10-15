using Microsoft.AspNetCore.Mvc;

namespace OdeToFood.Controllers
{

    // About
    //[Route("about")]
    // [action] tar metodnamnet som action
    // [Controller] tar classnamnet som sida att dirigera till.. tex  localhost/About/Address
    // Company/[controller]/[activio] company måste skrivas innan controller för att komma åt controllern.
    [Route("[Controller]/[action]")] 
    public class AboutController : Controller
    {

        //[Route("Phone")] // behöver inte göras om man anger [action] i controller route ovan.
        public string Phone()
        {
            return "070-000-000";
        }
        //[Route("address")] // behöver inte göras om man anger [action] i controller route ovan.
        public string Address()
        {
            return "Sweden";
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}