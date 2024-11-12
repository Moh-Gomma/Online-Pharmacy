// Models/CarouselImage.cs
using System.ComponentModel.DataAnnotations;

namespace pharmace.Models
{
    public class CarouselImage
    {
        public int Id { get; set; }
        [Required]
        public string ImageName { get; set; }
        public bool IsActive { get; set; }
        public string? Link { get; set; }
    }
}