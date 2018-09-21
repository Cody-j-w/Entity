using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace thewall.Models
{
    public class ViewModel
    {
        public User User{get;set;}
        public List<Message> Messages{get;set;}

    }
}