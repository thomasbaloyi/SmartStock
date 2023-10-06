using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using SmartStock.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace SmartStock.Controllers
{
    public class ProductController : Controller
    {
        private const string BaseUrl = "https://gendacproficiencytest.azurewebsites.net/API/ProductsAPI";
        private int paginator = 0;
        List<ProductModel> products = new(0);


        // GET: ProductController
        public async Task<IActionResult> Index(int? segment)
        {
            if (segment != null)
            {
                if (segment == 1)
                {
                    this.increment();
                }  else
                {
                    this.decrement();
                }
            } else
            {
                segment = 0;
            }

            if (products.Count == 0)
            {
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
            }
            
            return View(new ArraySegment<ProductModel>(products.ToArray(), this.paginator*10, 10));
        }

        private void increment()
        {
            this.paginator++;
            this.paginator = this.paginator;
        }

        private void decrement()
        {
            if (this.paginator != 0)
            {
                this.paginator--;
                this.paginator = this.paginator;
            }
            
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
                    request.Content = new StringContent(collection.ToJson(), Encoding.UTF8, "application/json");
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
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                using HttpResponseMessage response = await client.PutAsJsonAsync("ProductsAPI/"+id, collection.ToJson());

                Console.WriteLine(response.RequestMessage);

                if (response.IsSuccessStatusCode)
                {
                    Console.Write("Here");
                    return RedirectToActionPermanent(nameof(Details), new { id });
                }
                else
                {
                   
                    return RedirectToActionPermanent(nameof(Index));
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
