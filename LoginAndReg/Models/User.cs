using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace LoginAndReg.Models
{
    public class User
    {
        [Key]
        public int id{get; set;}

        [Required (ErrorMessage="Please provide your first name!")]
        [MinLength(2, ErrorMessage="First name must be at least 2 characters")]
        [MaxLength(24, ErrorMessage="First name cannot be more than 24 characters")]
        public string FirstName{get;set;}

        [Required (ErrorMessage="Please provide your last name!")]
        [MinLength(2, ErrorMessage="Last name must be at least 2 characters")]
        [MaxLength(24, ErrorMessage="Last name cannot be more than 24 characters")]
        public string LastName{get;set;}

        [Required (ErrorMessage="Please provide an E-mail address!")]
        [EmailAddress (ErrorMessage="Please provide a valid E-mail address!")]
        public string Email{get;set;}

        [Required(ErrorMessage="Please provide a password!")]
        [MinLength(8, ErrorMessage="Password must be at least 8 characters")]
        public string Password{get; set;}

        [Compare("Password", ErrorMessage="Passwords must match!")]
        [NotMapped]
        public string PassConfirm{get; set;}

        public DateTime CreatedAt{get;set;}
        public DateTime UpdatedAt{get;set;}

        public User()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }



    }
}