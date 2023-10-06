using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using SmartStock.Models;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SmartStock.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private const string BaseUrl = "https://gendacproficiencytest.azurewebsites.net/API/ProductsAPI";
        private int paginator = 0;
        List<ProductModel> products = new(0);

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }


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
                        this._logger.LogError("Failed to get products", response.ToString());
                        return RedirectToActionPermanent(nameof(Error));
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
                    this._logger.LogError("Failed to fetch product", response.ToString());
                    return RedirectToActionPermanent(nameof(Error));
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
                        this._logger.LogError("Failed to create product", response);
                        return RedirectToActionPermanent(nameof(Error));
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
                    this._logger.LogError("Failed to fetch product", response.ToString());
                    return RedirectToActionPermanent(nameof(Error));
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
                using HttpResponseMessage response = await client.PutAsJsonAsync("ProductsAPI/"+id, collection);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToActionPermanent(nameof(Details), new { id });
                }
                else
                {
                    this._logger.LogError("Failed to edit product", response.ToString());
                    return RedirectToActionPermanent(nameof(Error));
                }
            }


        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
                    this._logger.LogError("Failed to fetch product", response.ToString());
                    return RedirectToActionPermanent(nameof(Error));
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
                    this._logger.LogError("Failed to delete product", response);
                    return RedirectToActionPermanent(nameof(Error));
                }
            }
        }
    }
}
