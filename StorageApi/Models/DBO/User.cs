using StorageApi.Helper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StorageApi.Models.DBO
{
    public class User
    {
        [Key]
        public long Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        [NotMapped]
        public string Password { set => PasswordHash = AuthHelper.HashString(value); }
    }
}
