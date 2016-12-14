using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateCreated { get; set; }

        public ICollection<Plan> Plans { get; set; }
        public ICollection<GroceryItem> GroceryItems { get; set; }
        public ICollection<Bookmark> Bookmarks { get; set; }
        public ICollection<InventoryItem> InventoryItems { get; set; }
    }
}
