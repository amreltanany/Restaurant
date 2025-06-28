using Microsoft.AspNetCore.Mvc;
using Restaurant.Data;
using Restaurant.Models;

namespace Restaurant.Controllers
{
    public class ProductController : Controller
    {
        private Repository<Product> products;
        private Repository<Ingredient> ingredients;
        private Repository<Category> categories;
        private readonly IWebHostEnvironment _webHostEnvironment;



        public ProductController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            products = new Repository<Product>(context);
            ingredients = new Repository<Ingredient>(context);
            categories = new Repository<Category>(context);
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await products.GetAll());
        }

        [HttpGet]
        // Add/Edit action method for Product if id is 0 it will be Add operation, otherwise it will be Edit operation
        public async Task<IActionResult> AddEdit(int id)
        {
            //ViewBag is used to pass temporary data from the controller to the view.
            //Allows you to send extra data to the view without defining a class.
            ViewBag.Ingredients = await ingredients.GetAll();
            ViewBag.Categories = await categories.GetAll();
            if (id == 0)
            {
               
                ViewBag.Operation = "Add";
                return View(new Product());
            }
            else
            {
                ViewBag.Operation = "Edit";
                var product = await products.GetById(id, new QueryOptions<Product>
                {
                    Includes = "ProductIngredients.Ingredient,Category",
                    //OrderBy = p => p.Name
                });
                return View(product);

                //if (product == null)
                //{
                //    return NotFound();
                //}
                
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(Product product, int[] ingredientIds, int catId)
        {
            ViewBag.Ingredients = await ingredients.GetAll();
            ViewBag.Categories = await categories.GetAll();
            if (ModelState.IsValid) {

                //Check if a file was uploaded
                if (product.ImageFile != null) 
                {
                    // Get the path to the images folder inside wwwroot like this C:\MyApp\wwwroot\images
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                    // generates a random ID to avoid file name conflicts
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + product.ImageFile.FileName;

                    // Builds the full path on the server where the uploaded file will be saved using the unique file name
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // FileMode.Create: creates the file, overwrites if it exists
                    using (var fileStream = new FileStream(filePath,FileMode.Create))
                    {
                        // Takes the uploaded content
                        // Writes it to fileStream
                        await product.ImageFile.CopyToAsync(fileStream);
                    }
                    // Save the file name in the model to save to the database
                    product.ImageUrl = uniqueFileName;

                }

                if(product.ProductId == 0)
                {
                   
                    product.CategoryId = catId;

                    // Add ingredients to the product
                    foreach (int id in ingredientIds)
                    {
                        product.ProductIngredients?.Add(new ProductIngredient { IngredientId = id, ProductId = product.ProductId });
                    }
                    await products.Add(product);
                        return RedirectToAction("Index","Product");

                }
                else {
                    var existingProduct = await products.GetById(product.ProductId, new QueryOptions<Product>
                    {Includes = "ProductIngredients"});
                    if(existingProduct == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        // Update the existing product
                        existingProduct.Name = product.Name;
                        existingProduct.Description = product.Description;
                        existingProduct.Price = product.Price;
                        existingProduct.Stock = product.Stock;
                        existingProduct.CategoryId = catId;

                        // Clear existing ingredients and add new ones
                        existingProduct.ProductIngredients.Clear();
                        foreach (int id in ingredientIds)
                        {
                            existingProduct.ProductIngredients.Add(new ProductIngredient { IngredientId = id, ProductId = existingProduct.ProductId });
                        }
                        try
                        {
                            await products.Update(existingProduct);
                        }
                        catch (Exception ex)
                        {
                            // Handle concurrency exception if needed
                            ModelState.AddModelError("", "Unable to save changes. The product was modified by another user.");
                            return View(product);
                        }

                    }
                }
            }
            return RedirectToAction("Index", "Product"); 
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            try
            {
                await products.DeleteAsync(id);
                return RedirectToAction("Index", "Product");
            }
            catch
            {
                // Handle exception if needed, e.g., log it or show an error message
                ModelState.AddModelError("", "Unable to delete the product. It may be in use.");
                return RedirectToAction("Index");
            }
        }
}
}
