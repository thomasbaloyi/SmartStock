using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartStock.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SmartStock.Controllers
{
    public class ProductController : Controller
    {
        private const string BaseUrl = "https://gendacproficiencytest.azurewebsites.net/API/ProductsAPI";

        // GET: ProductController
        public async Task<IActionResult> Index()
        {
            List<ProductModel> products = new(0);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("ProductsAPI");

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    products = JsonSerializer.Deserialize<List<ProductModel>>(content);
                }
                else
                {
                    // Handle the error appropriately
                    ModelState.AddModelError(string.Empty, "Error retrieving data from the API.");
                }
            }


            return View(products);
        }


        // GET: ProductController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            ProductModel product = new();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("ProductsAPI/"+id);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    product  = JsonSerializer.Deserialize<ProductModel>(content);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error retrieving data from the API.");
                }
            }

            if (product != null)
            {
                Console.WriteLine(product.Name);
            }
      

            return View(product);
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
