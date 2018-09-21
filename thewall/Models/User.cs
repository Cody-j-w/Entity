using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace thewall.Models
{
    public class User
    {
        [Key]
        public int UserId{get;set;}
        [Required (ErrorMessage="Please provide your first name!")]
        [MinLength(2, ErrorMessage="First name must be at least 2 characters")]
        [MaxLength(24, ErrorMessage="First name cannot be more than 24 characters")]
        public string firstName{get;set;}

        [Required (ErrorMessage="Please provide your last name!")]
        [MinLength(2, ErrorMessage="Last name must be at least 2 characters")]
        [MaxLength(24, ErrorMessage="Last name cannot be more than 24 characters")]
        public string lastName{get;set;}

        [Required (ErrorMessage="Please provide an E-mail address!")]
        [EmailAddress (ErrorMessage="Please provide a valid E-mail address!")]
        public string email{get;set;}

        [Required(ErrorMessage="Please provide a password!")]
        [MinLength(8, ErrorMessage="Password must be at least 8 characters")]
        public string password{get; set;}

        [Compare("password", ErrorMessage="Passwords must match!")]
        [NotMapped]
        public string passConfirm{get; set;}

        public List<Message> messages{get;set;}
        public List<Comment> comments{get;set;}
        public DateTime createdAt{get;set;}
        public DateTime updatedAt{get;set;}

        public User()
        {
            createdAt = DateTime.Now;
            updatedAt = DateTime.Now;
            comments = new List<Comment>();
            messages = new List<Message>();
        }
    }
}