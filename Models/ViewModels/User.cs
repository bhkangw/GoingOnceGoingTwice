using System.ComponentModel.DataAnnotations;

namespace csharpbelt.Models
{
    public class RegisterUser : BaseEntity
    {
        [Display(Name = "Username")]
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Username { get; set; }

        [Display(Name = "First Name")]
        [Required]
        [MinLength(2)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        [MinLength(2)]
        public string LastName { get; set; }

        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "PW Confirm")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string Confirm { get; set; }
    }
    public class LoginUser : BaseEntity
    {
        [Required]
        [Display(Name = "Username")]
        // [EmailAddress(ErrorMessage = "Invalid Username")]
        public string LogUsername { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string LogPassword { get; set; }
    }
}