using System.ComponentModel.DataAnnotations.Schema;

namespace pharmace.Models
{
    public class Orders
    {
        public int Id { get; set; }
        public string? address { get; set; }
        public string? fname { get; set; }
        public string? sname { get; set; }
        public string? phone { get; set; }
        public string? sphone { get; set; }
        public string? email { get; set; }
        public DateTime Date { get; set; } 
        public float Totalprice { get; set; }
        public bool? states { get; set; }

        [ForeignKey("user")]
        public int userId { get; set; }
        public virtual userpermations user { get; set; }
    }
}
