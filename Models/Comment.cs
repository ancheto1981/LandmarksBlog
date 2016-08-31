using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LandmarksBlog.Models
{
    public class Comment
    {
        public Comment(Comment comment)
        {
            this.Date = DateTime.Now;
        }

        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public int PostId { get; set; }

        [Required]
        public virtual Post Post { get; set; }


    }
}