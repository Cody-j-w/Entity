using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace WeddingPlanner.Models
{
    public class Wedding
    {
        [Key]
        public int WeddingId{get;set;}

        [Required (ErrorMessage="Please provide the name of the first wedder!")]
        [MinLength(2, ErrorMessage="wedder name must be at least 2 characters")]
        [MaxLength(24, ErrorMessage="wedder name cannot be more than 24 characters")]
        public string wedderOne{get;set;}

        [Required (ErrorMessage="Please provide the name of the second wedder!")]
        [MinLength(2, ErrorMessage="wedder name must be at least 2 characters")]
        [MaxLength(24, ErrorMessage="wedder name cannot be more than 24 characters")]
        public string wedderTwo{get;set;}

        [Required]
        public DateTime weddingDay{get;set;}

        [Required]
        public string address{get;set;}
        public int UserId{get;set;}
        public User User{get;set;}
        public List<RSVP> guests{get;set;}
        public DateTime createdAt{get;set;}
        public DateTime updatedAt{get;set;}
        
    }
}