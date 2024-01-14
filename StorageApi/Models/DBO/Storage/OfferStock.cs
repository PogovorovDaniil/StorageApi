namespace StorageApi.Models.DBO.Storage
{
    public class OfferStock
    {
        public Offer Offer { get; set; }
        public Store Store { get; set; }
        public long Quantity { get; set; }
    }
}
