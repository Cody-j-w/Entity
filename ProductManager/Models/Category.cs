using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace ProductManager.Models
{
    public class Category
    {
        [Key]
        public int CategoryId {get; set;}
        public string name{get; set;}
        public List<ProductCategory> products{get;set;}
        public DateTime created_at{get; set;}
        public DateTime updated_at{get; set;}

        public Category()
        {
            products = new List<ProductCategory>();
            created_at = DateTime.Now;
            updated_at = DateTime.Now;

        }
    }
}