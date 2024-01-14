using System.ComponentModel.DataAnnotations;

namespace StorageApi.Models.DBO.Authorization
{
    public class User
    {
        [Key]
        public long Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
    }
}
