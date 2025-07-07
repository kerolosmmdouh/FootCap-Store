using System.ComponentModel.DataAnnotations;

namespace FootCap.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "كلمة المرور غير متطابقة")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Address { get; set; }
    }
}