using System.ComponentModel.DataAnnotations;

namespace StorageApi.Database.Models.Storage
{
    public class Offer
    {
        [Key]
        public long Id { get; set; }
        public Product Product { get; set; }
        public string Color { get; set; } = "default";
        public string Size { get; set; } = "default";
        public decimal Price { get; set; }
    }
}
