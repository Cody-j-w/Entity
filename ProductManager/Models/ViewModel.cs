using System.Collections.Generic;

namespace ProductManager.Models
{
    public class ViewModel
    {
        public Product product{get;set;}
        public Category category{get;set;}
        public List<Product> products{get;set;}
        public List<Category> categories{get;set;}
        public List<Product> UnusedProducts{get;set;}
        public List<Category> UnusedCategories{get;set;}
        public ProductCategory NewJoin{get;set;}
    }
}