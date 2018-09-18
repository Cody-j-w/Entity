using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProductManager.Models;

namespace ProductManager
{
    public class ProductCategory
    {

        [Key]
        public int PCID{get;set;}

        public int ProductId{get; set;}
        public Product Product{get; set;}

        public int CategoryId{get; set;}
        public Category Category{get; set;}
    }
}