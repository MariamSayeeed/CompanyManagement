using System.ComponentModel.DataAnnotations;

namespace MVC03.PL.Dtos
{
    public class SignInDto
    {
        [Required(ErrorMessage = "Password is Required !! ")]
        [DataType(DataType.Password)]
        //[RegularExpression(@"(?=(?:.*[A-Z]){3})(?=(?:.*[^a-zA-Z]){4})",
        //ErrorMessage = "Password must contain at least 3 uppercase letters and 4 non-alphabetic characters.")]

        public string Password { get; set; }


        [Required(ErrorMessage = "Email is Required !! ")]
        [EmailAddress]
        public string Email { get; set; }

        public bool RememberMe { get; set; }

    }
}
