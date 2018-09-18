using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Extensions;
using ProductManager.Models;

namespace ProductManager.Controllers
{
    public class ProductController : Controller
    {

        private Context _context;

        public ProductController(Context context)
        {
            _context = context;
        }

        [HttpGet("products/new")]
        public IActionResult New()
        {
            
            return View("NewProduct");
        }

        [HttpPost("products/create")]
        public IActionResult Create(Product product)
        {
            if(ModelState.IsValid)
            {
                Product ValidityTest = _context.Products.SingleOrDefault(v => v.name == product.name);
                
                    if(ValidityTest == null)
                    {
                        Product NewProduct = new Product()
                        {
                            name = product.name,
                            description = product.description,
                            price = product.price
                        };

                        _context.Products.Add(NewProduct);
                        _context.SaveChanges();

                        Product productID = _context.Products.SingleOrDefault(p => p.name == product.name);
                        
                        return RedirectToAction("OneProduct", new{id = productID.ProductId});
                    }
                    else
                    {
                        TempData["error"] = "This product was already created. You've been redirected to its page.";
                        return RedirectToAction("OneProduct", new{id = ValidityTest.ProductId});
                    }
            }
            else
            {
                return View("NewProduct");
            }
        }

        [HttpPost("products/newCategory")]
        public IActionResult AddCategory(int categoryid, int productid)
        {
            if(ModelState.IsValid)
            {
                Product product = _context.Products.SingleOrDefault(p => p.ProductId == productid);
                Category category = _context.Categories.SingleOrDefault(c => c.CategoryId == categoryid);


                ProductCategory productcategory = new ProductCategory();

                productcategory.Product = product;
                productcategory.Category = category;

                _context.ProductCategory.Add(productcategory);
                _context.SaveChanges();

                return RedirectToAction("OneProduct", new{id = product.ProductId});
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpGet("products/{id}")]
        public IActionResult OneProduct(int id)
        {
            
            List<Product> Products = _context.Products
                                .Include(l => l.categories)
                                .ThenInclude(c => c.Category)
                                .ToList();
            ViewModel ViewModel = new ViewModel();
            List<Category> Unused = new List<Category>();
            List<Category> Used = new List<Category>();
            int Marker;

            IEnumerable<Category> AllCategories = _context.Categories;
            foreach(var category in AllCategories)
            {
                Marker = 0;
                foreach(var product in category.products)
                {
                    
                    if(product.ProductId == id)
                    {
                        Marker++;
                    }
                }
                if(Marker == 0)
                {
                    Unused.Add(category);
                }
            }

            ViewModel.product = _context.Products.SingleOrDefault(p => p.ProductId == id);

            foreach(var product in Products)
            {
                if(product.ProductId == id)
                {
                    Console.WriteLine(product.name);
                    foreach(var category in product.categories)
                    {
                        Used.Add(category.Category);
                    }
                }
            }
            ViewModel.UnusedCategories = Unused;
            ViewModel.categories = Used;
            ViewModel.NewJoin = new ProductCategory();


            return View("OneProduct", ViewModel);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
