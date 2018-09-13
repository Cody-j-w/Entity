using System;
using System.ComponentModel.DataAnnotations;
using System.Collections;


namespace RESTaurant.Models
{

    public class NoFutureDates : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Restaur review = (Restaur)validationContext.ObjectInstance;

            if(review.Visit > DateTime.Now)
            {
                return new ValidationResult("Visit date must be from before today!");
            }
            return ValidationResult.Success;
            
        }
    
        
    }
    public class Restaur
    {
        [Key]
        public int id{get; set;}

        [Required (ErrorMessage="Please provide your name!")]
        [MinLength(2, ErrorMessage="Name must be at least two characters")]
        [MaxLength(24, ErrorMessage="Name cannot be more than 24 characters")]
        public string Name{get; set;}

        [Required(ErrorMessage="Please provide the name of the restaurant being reviewed!")]
        [MinLength(2, ErrorMessage="Restaurant name must be at least two characters")]
        [MaxLength(48, ErrorMessage="Restaurant name cannot be more than 48 characters")]
        public string Location{get; set;}

        [Required(ErrorMessage="Please provide your review!")]
        [MinLength(10, ErrorMessage="Review must be at least 10 characters")]
        [MaxLength(255, ErrorMessage="Review cannot be more than 255 characters")]
        public string Review{get; set;}

        

        [Required(ErrorMessage="Please provide the date of your visit!")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [NoFutureDates]
        public DateTime? Visit{get; set;}

        [Required]
        [Range(1,6)]
        public int? Rating{get; set;}
        public DateTime? Created_at{get; set;}
        public DateTime? Updated_at{get; set;}

        public Restaur()
        {
            Created_at = DateTime.Now;
            Updated_at = DateTime.Now;
        }
    }
}