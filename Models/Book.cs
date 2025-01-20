using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models
{
    public class Book
    {
        public int Id { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string? Title { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string? Author { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime ModifiedDate { get; set; } = DateTime.Now;

        // Add Quantity
        //public int Quantity { get; set; }
    }
}
