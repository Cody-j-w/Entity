using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace WeddingPlanner.Models
{
    public class ViewModel
    {
        public int? PersonalId{get;set;}
        public User User{get;set;}
        public Wedding Wedding{get;set;}
        
        public List<Wedding> Weddings{get;set;}
        public List<User> Users{get;set;}
        public List<RSVP> RSVPs{get;set;}

    }
}