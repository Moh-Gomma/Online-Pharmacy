using System.ComponentModel.DataAnnotations.Schema;

namespace pharmace.Models
{
    public class Cart
    {
        [ForeignKey("proudect")]
        public int proudectId { get; set; }
        public virtual Proudect proudect { get; set; }

        [ForeignKey("user")]
        public int userId { get; set; }
        public virtual userpermations user { get; set; }

        public int count { get; set; }
    }
}
