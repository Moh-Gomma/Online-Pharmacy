using System.ComponentModel.DataAnnotations.Schema;

namespace pharmace.Models
{
    public class Proudect
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public int count { get; set; }

        public string Description { get; set; }
        public string? image { get; set; }
        public float Price { get; set; }
        public float? Pricecycle { get; set; }

        [ForeignKey("category")]
        public int categoryId { get; set; }
        public virtual Category category { get; set; }



    }
}
