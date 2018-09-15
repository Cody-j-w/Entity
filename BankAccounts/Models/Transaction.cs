using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;

namespace BankAccounts.Models
{
    public class Transaction
    {
        [Key]
        public int Id{get;set;}
        public decimal value{get;set;}
        public DateTime createdAt{get;set;}
        public DateTime updatedAt{get;set;}

        [ForeignKey("Id")]
        public int userId{get;set;}
        public User user{get;set;}

        public Transaction()
        {
            createdAt = DateTime.Now;
            updatedAt = DateTime.Now;
        }
        
    }

}