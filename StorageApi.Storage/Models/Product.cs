using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace StorageApi.Storage.Models
{
    public class PostProduct
    {
        public string Name { get; set; }
        public long BrandId { get; set; }
        public IEnumerable<Offer> Offers { get; set; }
        public class Offer
        {
            [AllowNull]
            public string Color { get; set; }
            [AllowNull]
            public string Size { get; set; }
            public decimal Price { get; set; }
        }
    }
    public class GetProduct
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long BrandId { get; set; }
        public string BrandName { get; set; }
        public IEnumerable<Offer> Offers { get; set; }
        public class Offer
        {
            public long Id { get; set; }
            [AllowNull]
            public string Color { get; set; }
            [AllowNull]
            public string Size { get; set; }
            public decimal Price { get; set; }
        }
    }
}