using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace thewall.Models
{
    public class Message
    {
        public int MessageId{get;set;}
        public string message{get;set;}
        public List<Comment> comments{get;set;}
        public int UserId{get;set;}
        public User User{get;set;}

        public DateTime createdAt{get;set;}
        public DateTime updatedAt{get;set;}

        public Message()
        {
            createdAt = DateTime.Now;
            updatedAt = DateTime.Now;
            comments = new List<Comment>();
        }
    }
}