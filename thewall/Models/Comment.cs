using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace thewall.Models
{
    public class Comment
    {
        [Key]
        public int CommentId{get;set;}
        public string comment{get;set;}

        public int UserId{get;set;}
        public User User{get;set;}

        public int MessageId{get;set;}
        public Message Message{get;set;}

        public DateTime createdAt{get;set;}
        public DateTime updatedAt{get;set;}

        public Comment()
        {
            createdAt = DateTime.Now;
            updatedAt = DateTime.Now;
        }

    }
}