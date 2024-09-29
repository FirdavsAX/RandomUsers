using Microsoft.AspNetCore.Mvc;
using RandomUsers.Data;
using RandomUsers.Models;

namespace RandomUsers.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDataSeeder _dataSeeder;

        public HomeController(IDataSeeder dataSeeder)
        {
            _dataSeeder=dataSeeder;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetUsers(string region, int count = 20, int seed = 123456, int errors = 0, int page = 1)
        {
            var users = _dataSeeder.GenerateUsers(region, count, seed + page, errors);
            return Json(users);
        }
    }
}
