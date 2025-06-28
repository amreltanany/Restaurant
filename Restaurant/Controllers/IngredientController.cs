using Microsoft.AspNetCore.Mvc;
using Restaurant.Data;
using Restaurant.Models;

namespace Restaurant.Controllers
{
    public class IngredientController : Controller
    {
        
        //generic repository class designed to handle data access so you don't have a fat controller
        private Repository<Ingredient> ingredients;

        // Constructor to inject the ApplicationDbContext ,This allows the repository to interact with the database.
        public IngredientController(ApplicationDbContext context)
        {
            ingredients = new Repository<Ingredient>(context);
        }
        

        public async Task<IActionResult> Index()
        {
            return View(await ingredients.GetAll());
        }

        public async Task<IActionResult> Details(int id)
        {
            return View(await ingredients.GetById(id, new QueryOptions<Ingredient>() { Includes = "ProductIngredients.Product" }));
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Ingredient,Name")] Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                await ingredients.Add(ingredient);
                return RedirectToAction(nameof(Index));
            }
            return View(ingredient);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
           return View(await ingredients.GetById(id, new QueryOptions<Ingredient> { Includes="ProductIngredients.Product"}));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Ingredient ingredient)
        {
            await ingredients.DeleteAsync(ingredient.IngredientId);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            return View(await ingredients.GetById(id, new QueryOptions<Ingredient> { Includes = "ProductIngredients.Product" }));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                await ingredients.Update(ingredient);
                return RedirectToAction(nameof(Index));
            }
            return View(ingredient);
        }
}
}
