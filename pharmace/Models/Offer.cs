using System.ComponentModel.DataAnnotations.Schema;

namespace pharmace.Models
{
    public class Offer
    {
        public int Id { get; set; }

        public int presentage { get; set; }

        [ForeignKey("proudect")]
        public int proudectId { get; set; }
        public virtual Proudect proudect { get; set; }
    }
}
