using System.Linq;
using aspnetcoreapp_efcore_inherited_entity_id_problem.Data;
using Microsoft.AspNetCore.Mvc;

namespace aspnetcoreapp_efcore_inherited_entity_id_problem.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Constructor
        /// </summary>
        public HomeController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            // Uncomment the case you would like to test...

            // 1st problem:
            // This causes error "ArgumentException: An item with the same key has already been added. Key: 1".
            _dbContext.Cat.Add(
                    new Cat
                    {
                        Name = "Tom",
                    });
            _dbContext.SaveChanges();

            // 2nd problem:
            // This inserts new Dog with id 1 and replaces Cat with id 1.
            //_dbContext.Dog.Add(
            //    new Dog
            //    {
            //        Name = "Laika"
            //    });
            //_dbContext.SaveChanges();


            return View(_dbContext.Animal.ToList());
        }
    }
}
