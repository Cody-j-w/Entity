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
    public class CategoryController : Controller
    {

        private Context _context;

        public CategoryController(Context context)
        {
            _context = context;
        }

        [HttpGet("categories/new")]
        public IActionResult New()
        {
            
            return View("NewCategory");
        }

        [HttpPost("categories/create")]
        public IActionResult Create(Category category)
        {
            if(ModelState.IsValid)
            {
                Category NewCategory = new Category()
                {
                    name = category.name
                };

                _context.Categories.Add(NewCategory);
                _context.SaveChanges();

                Category RouteId = _context.Categories.SingleOrDefault(c => c.name == category.name);

                return RedirectToAction("OneCategory", new{id = RouteId.CategoryId});
            }
            else
            {
                return View("NewCategory");
            }
        }

        [HttpPost("categories/newProduct")]
        public IActionResult AddProduct(int categoryid, int productid)
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

                return RedirectToAction("OneCategory", new{id = category.CategoryId});
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpGet("categories/{id}")]
        public IActionResult OneCategory(int id)
        {
            List<Category> Categories = _context.Categories
                                .Include(l => l.products)
                                .ThenInclude(p => p.Product)
                                .ToList();
            ViewModel ViewModel = new ViewModel();
            List<Product> Unused = new List<Product>();
            List<Product> Used = new List<Product>();
            int Marker;

            IEnumerable<Product> AllProducts = _context.Products;

            foreach(var product in AllProducts)
            {
                Marker = 0;
                foreach(var category in product.categories)
                {
                    if(category.CategoryId == id)
                    {
                        Marker++;
                    }
                }
                if(Marker == 0)
                {
                    Unused.Add(product);
                }
            }

            ViewModel.category = _context.Categories.SingleOrDefault(c => c.CategoryId == id);
            foreach(var category in Categories)
            {
                if(category.CategoryId == id)
                {
                    foreach(var product in category.products)
                    {
                        Used.Add(product.Product);
                    }
                }
            }

            ViewModel.UnusedProducts = Unused;
            ViewModel.products = Used;
            ViewModel.NewJoin = new ProductCategory();
            
            return View("OneCategory", ViewModel);
        }
    }
}