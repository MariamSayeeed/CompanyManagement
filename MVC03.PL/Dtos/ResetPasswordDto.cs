using System.ComponentModel.DataAnnotations;

namespace MVC03.PL.Dtos
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "Password is Required !! ")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }


        [Required(ErrorMessage = "ConfirmPassword is Required !! ")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "ConfirmPassword does not match the Password ")]
        public string ConfirmPassword { get; set; }


    }
}
