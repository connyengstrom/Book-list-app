using System;
using System.ComponentModel.DataAnnotations;

namespace BookQuotesApi.Models
{
    public class Book
    {
        public int Id { get; set; }
        
        [Required]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        public string Author { get; set; } = string.Empty;
        
        [Required]
        public string Genre { get; set; } = string.Empty;

        [Required]
        public DateTime PublishedDate { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
