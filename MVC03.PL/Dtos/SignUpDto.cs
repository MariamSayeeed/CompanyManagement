using System.ComponentModel.DataAnnotations;

namespace MVC03.PL.Dtos
{
    public class SignUpDto
    {
        [Required(ErrorMessage ="UserName is Required !! ")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "FirstName is Required !! ")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "LastName is Required !! ")]

        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is Required !! ")]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Password is Required !! ")]
        [DataType(DataType.Password)]
        [RegularExpression(@"(?=(?:.*[A-Z]){3})(?=(?:.*[^a-zA-Z]){4})",
        ErrorMessage = "Password must contain at least 3 uppercase letters and 4 non-alphabetic characters.")]

        public string Password { get; set; }

        [Required(ErrorMessage = "ConfirmPassword is Required !! ")]
        [DataType(DataType.Password)]
        [RegularExpression(@"(?=(?:.*[A-Z]){3})(?=(?:.*[^a-zA-Z]){4})",
        ErrorMessage = "Password must contain at least 3 uppercase letters and 4 non-alphabetic characters.")]
        [Compare(nameof(Password), ErrorMessage = "ConfirmPassword does not match the Password ") ]
        public string ConfirmPassword { get; set; }

        public bool IsAgree { get; set; }
    }
}
