using System.ComponentModel.DataAnnotations;

namespace StorageApi.Models.DBO.Storage
{
    public class Store
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
