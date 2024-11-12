using System.ComponentModel.DataAnnotations.Schema;

namespace pharmace.Models
{
    public class orderprodect
    {
        [ForeignKey("order")]
        public int orderId { get; set; }
        public virtual Orders order { get; set; }

        [ForeignKey("proudect")]
        public int proudectId { get; set; }
        public virtual Proudect proudect { get; set; }

        public int count { get; set; }

    }
}
