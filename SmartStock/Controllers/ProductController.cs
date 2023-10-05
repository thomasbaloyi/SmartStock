using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using SmartStock.Models;
using System.Net.Http.Headers;
using System.Reflection;
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
        public async Task<IActionResult> Create(IFormCollection collection)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = BaseUrl + "/ProductsAPI";

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
                    request.Content = new StringContent(collection.ToJson(), System.Text.Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(url, request.Content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToActionPermanent(nameof(Index));
                    }
                    else
                    {
                        Console.WriteLine(response.StatusCode);
                        Console.WriteLine(response.Content.ToJson());
                        return RedirectToActionPermanent(nameof(HomeController));
                    }
                }
            }
            catch
            {
                return RedirectToActionPermanent(nameof(HomeController));
            }
        }

        // GET: ProductController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {

            ProductModel product = new();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("ProductsAPI/" + id);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    product = JsonSerializer.Deserialize<ProductModel>(content);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error retrieving data from the API.");
                }
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormCollection collection)
        {

            using (HttpClient client = new HttpClient())
            {

                string url = BaseUrl + "/ProductsAPI/" + id;

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, url);
                request.Content = new StringContent(collection.ToJson(), System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToActionPermanent(nameof(Index));
                }
                else
                {
                    Console.WriteLine(response.StatusCode);
                    Console.WriteLine(response.Content.ToJson());
                    return RedirectToActionPermanent(nameof(Details), new { id });
                }
            }

            
        }


        // GET: ProductController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            ProductModel product = new();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("ProductsAPI/" + id);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    product = JsonSerializer.Deserialize<ProductModel>(content);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error retrieving data from the API.");
                }
            }

            return View(product);
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.DeleteAsync("ProductsAPI/" + id);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToActionPermanent(nameof(Index));
                }
                else
                {
                    return RedirectToActionPermanent(nameof(Details), new { id });
                }
            }
        }
    }
}
