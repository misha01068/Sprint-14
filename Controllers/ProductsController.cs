using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductsWithRouting.Models;
using ProductsWithRouting.Services;

namespace ProductsWithRouting.Controllers
{
    [Route("products")]
    [Route("items")]

    public class ProductsController : Controller
    {

        private List<Product> myProducts;

        public ProductsController(Data data)
        {
            myProducts = data.Products;
        }

        [HttpGet("")]
        [HttpGet("index")]

        public IActionResult Index(int filterId, string filtername)
        {
            IEnumerable<Product> products = myProducts;

            if (filterId > 0)
                products = products.Where(p => p.Id == filterId);

            if (!string.IsNullOrWhiteSpace(filtername))
                products = products.Where(p => p.Name != null &&
                    p.Name.Contains(filtername, StringComparison.OrdinalIgnoreCase));

            return View(products.ToList());
        }
        [HttpGet("{id:int}")]
        public IActionResult View(int id)
        {
            var product = myProducts.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return RedirectToAction("NotFoundPage");
            return View(product);
        }
        [HttpGet("edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var product = myProducts.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return RedirectToAction("NotFoundPage");
            return View(product);
        } 
        [HttpPost("edit/{id:int}")]
        public IActionResult Edit(int id, Product product)
        {
            var existing = myProducts.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return RedirectToAction("NotFoundPage");
            existing.Name = product.Name;
            existing.Description = product.Description;
            return RedirectToAction("Index");
        }

        [HttpPost("create")]
        [HttpPost("new")]
        public IActionResult Create(Product product)
        {
            product.Id = myProducts.Any() ? myProducts.Max(p => p.Id) + 1 : 1;
            myProducts.Add(product);
            return RedirectToAction("Index");
        }

        [HttpGet("create")]
        [HttpGet("new")]
        public IActionResult Create()
        {
            var nextId = myProducts.Any() ? myProducts.Max(p => p.Id) + 1 : 1;
            return View(new Product { Id = nextId });
        }

        [HttpGet("delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            var product = myProducts.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return RedirectToAction("NotFoundPage");
            myProducts.Remove(product);
            return RedirectToAction("Index");
        }

        [Route("error")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet("notfound")]
        public IActionResult NotFoundPage()
        {
            return View();
        }
    }
}
