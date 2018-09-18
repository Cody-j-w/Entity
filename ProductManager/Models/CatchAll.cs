using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace ProductManager.Models
{
    public class CatchAll
    {
        public IEnumerable<Product> AllProducts{get;set;}
        public IEnumerable<Category> AllCategories{get;set;}
    }
}