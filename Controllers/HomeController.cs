using aspnetcoreapp_efcore_inherited_entity_id_problem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.InMemory.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.InMemory.Storage.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Linq;

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
            // Workaround according to https://github.com/dotnet/efcore/issues/19854#issuecomment-596011059
            int maxId = _dbContext.Animal.Max(x => x.Id);
            if (maxId == 2)
            {
                BumpGenerator(_dbContext, typeof(AnimalBase), nameof(AnimalBase.Id), 2);
            }

            // Uncomment the case you would like to test...

            // 1st problem:
            // NOT A PROBLEM ANYMORE THANKS TO THE WORKAROUND This causes error "ArgumentException: An item with the same key has already been added. Key: 1".
            _dbContext.Cat.Add(
                   new Cat
                   {
                       Name = "Tom",
                   });
            _dbContext.SaveChanges();

            // 2nd problem:
            // NOT A PROBLEM ANYMORE THANKS TO THE WORKAROUND This inserts new Dog with id 1 and replaces Cat with id 1.
            _dbContext.Dog.Add(
                new Dog
                {
                    Name = "Laika"
                });
            _dbContext.SaveChanges();

            return View(_dbContext.Animal.ToList());
        }

        private static void BumpGenerator(
            DbContext context, Type entityClrType, string propertyName, int newLowBound)
        {
            var entityType = context.Model.FindEntityType(entityClrType);
            var property = entityType.FindProperty(propertyName);
            var options = context.GetService<IDbContextOptions>().FindExtension<InMemoryOptionsExtension>();
            var inMemoryStore = context.GetService<IInMemoryStoreCache>().GetStore(options.StoreName);
            var generator = inMemoryStore.GetIntegerValueGenerator<int>(property);

            var values = new object[entityType.GetDeclaredProperties().Count()];
            values[property.GetIndex()] = newLowBound;
            generator.Bump(values);
        }
    }
}
