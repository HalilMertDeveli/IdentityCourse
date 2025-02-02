using System.ComponentModel.DataAnnotations;

namespace IdentityVersion2.Models
{
    public class UserSingInModel
    {
        [Required(ErrorMessage = "Kullanıcı ismi gereklidir")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Kullanıcı şifresi gereklidir")]
        public string Password { get; set; }
    }
}
