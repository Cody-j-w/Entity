using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace ProductManager.Models
{
    public class Product
    {
        [Key]
        public int ProductId {get; set;}
        public string name{get; set;}
        public string description{get; set;}
        public decimal? price{get; set;}
        public List<ProductCategory> categories{get;set;}
        public DateTime created_at{get; set;}
        public DateTime updated_at{get; set;}

        public Product()
        {
            categories = new List<ProductCategory>();
            created_at = DateTime.Now;
            updated_at = DateTime.Now;

        }
    }
}