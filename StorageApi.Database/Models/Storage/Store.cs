using System.ComponentModel.DataAnnotations;

namespace StorageApi.Database.Models.Storage
{
    public class Store
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
