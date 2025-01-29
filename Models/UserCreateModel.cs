using System.ComponentModel.DataAnnotations;

namespace IdentityVersion2.Models
{
    public class UserCreateModel
    {
        [Required(ErrorMessage = "Kullanıcı adı gereklidir")] 
        public string UserName { get; set; }
        [EmailAddress(ErrorMessage = "Lütfen bir email formatı giriniz")]
        [Required(ErrorMessage = "Email adresi gereklidir")]
        public string  Email { get; set; }
        [Required(ErrorMessage = "Parola alanı gereklidir")]
        public string  Password { get; set; }
        [Compare("Password",ErrorMessage = "Parolar eşleşmiyor")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Cinsiyet gereklidir")]
        public string Gender { get; set; }  
    }
}
