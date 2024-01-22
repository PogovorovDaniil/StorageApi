namespace StorageApi.Storage.Models
{
    public class OfferStock
    {
        public long OfferId { get; set; }
        public long StoreId { get; set; }
        public long Quantity { get; set; }
    }

    public class StoreStock
    {
        public long StoreId { get; set; }
        public long Quantity { get; set; }
    }
}
