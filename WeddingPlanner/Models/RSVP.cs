using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace WeddingPlanner.Models
{
    public class RSVP
    {
        [Key]
        public int RSVPID{get; set;}

        public int UserId{get;set;}
        public User User{get;set;}
        public int WeddingId{get;set;}
        public Wedding Wedding{get;set;}
    }
}