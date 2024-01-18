using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StorageApi.Database.Models.Storage
{
    public class Product
    {
        [Key]
        public long Id { get; set; }
        public Brand Brand { get; set; }
        public string Name { get; set; }
        public ICollection<Offer> Offers { get; } = new List<Offer>();
    }
}