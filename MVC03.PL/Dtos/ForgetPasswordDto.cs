using System.ComponentModel.DataAnnotations;

namespace MVC03.PL.Dtos
{
    public class ForgetPasswordDto
    {
        [Required(ErrorMessage = "Email is Required !! ")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
