using System.ComponentModel.DataAnnotations;

namespace StorageApi.Database.Models.Storage
{
    public class Brand
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
