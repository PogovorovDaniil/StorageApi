using System.Diagnostics.CodeAnalysis;

namespace StorageApi.Storage.Models
{
    public class PostOffer
    {
        public long ProductId { get; set; }
        [AllowNull]
        public string Color { get; set; }
        [AllowNull]
        public string Size { get; set; }
        public decimal Price { get; set; }
    }
    public class GetOffer
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        [AllowNull]
        public string Color { get; set; }
        [AllowNull]
        public string Size { get; set; }
        public decimal Price { get; set; }
    }
}
